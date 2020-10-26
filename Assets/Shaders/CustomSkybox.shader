Shader "Skybox/Custom Skybox"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        _Color3 ("Color 3", Color) = (1, 1, 1, 1)
        _Color4 ("Color 4", Color) = (1, 1, 1, 1)
        _Color5 ("Color 5", Color) = (1, 1, 1, 1)
        _ThroughVector ("Through Vector", Vector) = (1, 0, 0, 0)
        _UpVector ("Up Vector", Vector) = (0, 1, 0, 0)
        _Proportion ("Proportion", Float) = 1.0
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    half4 _Color1;
    half4 _Color2;
    half4 _Color3;
    half4 _Color4;
    half4 _Color5;
    half4 _ThroughVector;
    half4 _UpVector;
    half _Proportion;
    
    v2f vert (appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos (v.position);
        o.texcoord = v.texcoord;
        return o;
    }

    float gradient_amount(float x, float y, int increment) {
        float offset = increment * 0.2f;
        float amount = pow(x * .5f + .5f, 1.4f) * pow(1.01 - y, 6 * _Proportion) + sin(_Time) * .02f; 
        return min(max(1 - (offset - amount) * 5, 0), 1);
    }
    
    fixed4 frag (v2f i) : COLOR
    {
        float x = dot (normalize (i.texcoord), normalize(_ThroughVector));
        float y = dot(normalize(i.texcoord), _UpVector);
        return lerp(lerp(lerp(lerp(
            _Color1, _Color2, pow(x * .5f + .5f, 2)),
            _Color3, gradient_amount(x, y, 2)),
            _Color4, gradient_amount(x, y, 3)),
            _Color5, gradient_amount(x, y, 4));
        // return lerp(_Color1, _Color2, x * .5f + .5f);
        // half amount = pow(2 * x - 1, .25f) * pow(1 - y, 2);
        // float amount = pow(x * .5f + .5f, .7f) * pow(1 - y, 2) + sin(_Time) * .1f; 
        //return lerp(_Color1, _Color5, amount);
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    CustomEditor "GradientSkyboxInspector"
}