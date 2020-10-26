Shader "Unlit/StarShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _BGColor1 ("BG Color 1", Color) = (1, 1, 1, 1)
        _BGColor2 ("BG Color 2", Color) = (1, 1, 1, 1)
        _FlickerSpeed ("Flicker Speed", Float) = 1.0
        _MaxAlpha ("Max Alpha", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            half4 _MainColor;
            half4 _BGColor1;
            half4 _BGColor2;
            half _FlickerSpeed;
            half _MaxAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float x = abs(1 - i.uv.x * 2);
                float y = abs(1 - i.uv.y * 2);
                float value1 =  min(max(1 - pow(x * x * 100 + y * y * 100, 2), 0) + max(1 - pow(x, 0.3f) - pow(y, 0.3f), 0), 1); 
                float4 c = lerp(lerp(_BGColor1, _BGColor2, sin(_Time * _FlickerSpeed * 3)), _MainColor, value1);
                c.a *= value1;
                c.a = min(c.a, _MaxAlpha);
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
}
