// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Mountain"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        _ThroughVector ("Through Vector", Vector) = (1, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            float4 _Color1;
            float4 _Color2;
            half4 _ThroughVector;
            
            v2f vert (appdata v)
            {
                 v2f o;
                 o.position = UnityObjectToClipPos (v.vertex);
                 o.texcoord = v.texcoord;
                 return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float x = dot(i.texcoord, normalize(_ThroughVector));
                fixed4 col = lerp(_Color1, _Color2, max(min(x, 1), 0));
                // fixed4 col = _Color1;
                return col;
            }
            ENDCG
        }
    }
}