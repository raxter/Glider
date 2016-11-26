using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoMaterialThingsBasedOnDepth : MonoBehaviour
{
    [Header("Background Colours")]
    public Color[] rmBackgroundColours;
    public float backgroundPeriod = 7;

    [Header("Light Colours")]
    public Color[] rmLightColours;
    public float lightPeriod = 11;

    [Header("Shadow Colours")]
    public Color[] rmShadowColours;
    public float shadowPeriod = 13;

    [Header("Noise and offset / Other")]
    public float offsetPerDepth;

    public Texture2D [] rmNoiseTextures;

    public float texturePeriod = 121;

    public AnimationCurve randomRangePicker;

    [Header("Div & Input Ratio")]
    public float divThicknessMin = 0.08f;
    public float divThicknessMax = 0.205f;
    public float divThicknessPeriod = 150;

    public float divRangeMin = 0.08f;
    public float divRangeMax = 0.245f;
    public float divRangePeriod = 161;

    public float inputRatioMin = -0.1f;
    public float inputRatioMax = 0.363f;
    public float inputRatioPeriod = 154;

    [Header("Materials")]
    public Material rmMaterial;
    public Material backgroundMaterial;


    [Header("Target")]
    public Transform depthTarget;

    float targetDivThickness;
    float targetDivRange;
    float targetInputRatio;

    float currentDivThickness;
    float currentDivRange;
    float currentInputRatio;

    Color targetRMBackground;
    Color targetRMLight;
    Color targetRMShadow;

    Color currentRMBackground;
    Color currentRMLight;
    Color currentRMShadow;

    float depthOffset;
    public float startDepth = 100;
    void Start()
    {
        depthOffset = Random.Range(100, 10000);
        
        targetRMBackground = new Color(0, 0, 0, 0);
        targetRMLight = new Color(0, 0, 0, 0);
        targetRMShadow = new Color(0, 0, 0, 0);

        lastBackgroundChange = depthOffset + startDepth;
        lastLightChange = depthOffset + startDepth;
        lastShadowChange = depthOffset + startDepth;

        PickNewTexture(depthOffset);
    }

    float lastBackgroundChange = -1;
    float lastLightChange = -1;
    float lastShadowChange = -1;
    float lastTextureChange = -1;

    float lastDivThickness = -1;
    float lastDivRange = -1;
    float lastInputRatio = -1;

    void PickNewThingyCheck<T>(float depth, ref float lastChange, float period, ref T targetVar, T[] array)
    {
        if (depth > lastChange + period)
        {
            targetVar = array[Random.Range(0, array.Length)];
            lastChange = depth;
        }
    }
    void PickNewThingyCheckRand(float depth, ref float lastChange, float period, ref float targetVar, float min, float max)
    {
        if (depth > lastChange + period)
        {
            targetVar = Mathf.Lerp(min, max, randomRangePicker.Evaluate(Random.value));
            lastChange = depth;
        }
    }

    //void PickNewBackground(float depth)
    //{
    //    targetRMBackground = rmBackgroundColours[Random.Range(0, rmBackgroundColours.Length)];
    //    lastBackgroundChange = depth;
    //}
    //void PickNewLight(float depth)
    //{
    //    targetRMLight = rmLightColours[Random.Range(0, rmLightColours.Length)];
    //    lastLightChange = depth;
    //}
    //void PickNewShadow(float depth)
    //{
    //    targetRMShadow = rmShadowColours[Random.Range(0, rmShadowColours.Length)];
    //    lastShadowChange = depth;
    //}
    void PickNewTexture(float depth)
    {
        Texture2D targetRMTex = rmNoiseTextures[Random.Range(0, rmNoiseTextures.Length)];
        StartCoroutine(LerpToNewTexture(targetRMTex));
        lastTextureChange = depth;
    }

    float oldDepth;
    // Update is called once per frame
    void Update()
    {
        float depth = -depthTarget.transform.position.y+depthOffset;

        backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(depth * offsetPerDepth, 0));

        currentRMBackground = Color.Lerp(currentRMBackground, targetRMBackground, Time.deltaTime * 3);
        currentRMLight = Color.Lerp(currentRMLight, targetRMLight, Time.deltaTime * 3);
        currentRMShadow = Color.Lerp(currentRMShadow, targetRMShadow, Time.deltaTime * 3);

        currentDivThickness = Mathf.Lerp(currentDivThickness, targetDivThickness, Time.deltaTime * 3);
        currentDivRange = Mathf.Lerp(currentDivRange, targetDivRange, Time.deltaTime * 3);
        currentInputRatio = Mathf.Lerp(currentInputRatio, targetInputRatio, Time.deltaTime * 3);


        //Debug.Log((oldDepth % backgroundPeriod) +">"+ (depth % backgroundPeriod));

        //if (depth > lastBackgroundChange + backgroundPeriod)
        //    PickNewBackground(depth);
        //if (depth > lastLightChange + lightPeriod)
        //    PickNewLight(depth);
        //if (depth > lastShadowChange + shadowPeriod)
        //    PickNewShadow(depth);


        if (depth > lastTextureChange + texturePeriod)
            PickNewTexture(depth);

        // Colours
        PickNewThingyCheck(depth, ref lastBackgroundChange, backgroundPeriod, ref targetRMBackground, rmBackgroundColours);
        PickNewThingyCheck(depth, ref lastLightChange, lightPeriod, ref targetRMLight, rmLightColours);
        PickNewThingyCheck(depth, ref lastShadowChange, shadowPeriod, ref targetRMShadow, rmShadowColours);

        // div and input ratio
        PickNewThingyCheckRand(depth, ref lastDivThickness, divThicknessPeriod, ref targetDivThickness, divThicknessMin, divThicknessMax);
        PickNewThingyCheckRand(depth, ref lastDivRange, divRangePeriod, ref targetDivRange, divRangeMin, divRangeMax);
        PickNewThingyCheckRand(depth, ref lastInputRatio, inputRatioPeriod, ref targetInputRatio, inputRatioMin, inputRatioMax);

        targetInputRatio = Mathf.Max(0, targetInputRatio);

        rmMaterial.SetColor("_ColorBackground", currentRMBackground);
        rmMaterial.SetColor("_ColorLight", currentRMLight);
        rmMaterial.SetColor("_ColorShadow", currentRMShadow);

        rmMaterial.SetFloat("_DivisionThinckness", currentDivThickness);
        rmMaterial.SetFloat("_DivisionRange", currentDivRange);
        rmMaterial.SetFloat("_InputRatio", currentInputRatio);


        oldDepth = depth;
    }

    public float textureTransTime = 0.66f;
    bool doubleRunGuard = false;
    IEnumerator LerpToNewTexture(Texture2D newTex)
    {
        if (doubleRunGuard)
            yield break;
        doubleRunGuard = true;

        rmMaterial.SetTexture("_NextTexture", newTex);

        float time = 0;
        while (time < textureTransTime)
        {
            rmMaterial.SetFloat("_TextureLerp", time/textureTransTime);


            time += Time.deltaTime;

            yield return null;
        }


        rmMaterial.SetFloat("_TextureLerp", 0);
        rmMaterial.SetTexture("_Texture", newTex);
        doubleRunGuard = false;

    }
}
