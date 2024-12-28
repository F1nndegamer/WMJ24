Shader "Custom/InvertColorsWithScaling"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {} // Not used but defined for compatibility
        _Scale("UV Scale", Vector) = (1, 1, 0, 0) // Scale and offset for UV adjustment
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }

        // Grab the screen texture
        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _GrabTexture;   // The texture containing the screen capture
            float4 _Scale;           // Scale (x, y) and offset (z, w) for UVs

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Apply scaling and offset to UV coordinates
                float2 scaledUV = i.uv * _Scale.xy + _Scale.zw;

                // Sample the screen texture at the scaled UV coordinates
                fixed4 screenColor = tex2D(_GrabTexture, scaledUV);

                // Invert the colors
                screenColor.rgb = 1.0 - screenColor.rgb;

                return screenColor;
            }
            ENDCG
        }
    }
}
