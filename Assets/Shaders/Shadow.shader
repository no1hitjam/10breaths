Shader "Unlit/Shadow"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = abs(1 - i.uv.x * 2);
                float y = abs(1 - i.uv.y * 2);
                float center_lerp = max(1 - pow((x * x) * 2000 + (y * y) * 2000, .8f), 0);
                float4 c = _Color;
                float value = max(1 - pow((x * x) + (y * y), .5f) + _SinTime.x * .1f, 0);
                c.a *= value * .99f + center_lerp * .01f;
                return c;
            }
            ENDCG
        }
    }
}
