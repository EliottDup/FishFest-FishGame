Shader "FISH/Ditherer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackgroundCol ("Background Color", Color) = (0, 0, 0, 0)

        _StartRange ("StartRange", float) = 0
        _EndRange ("EndRange", float) = 10
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _StartRange;
            float _EndRange;

            float4 _BackgroundCol;
            
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            
            static float4 M4[4] = {
                float4(0.0 , 8.0 , 2.0 , 10.0),
                float4(12.0, 4.0 , 14.0, 6.0 ),
                float4(3.0 , 11.0, 1.0 , 9.0 ),
                float4(15.0, 7.0 , 13.0, 5.0 )
            };

            static float M8[8][8] = {
                {0 , 32,  8, 40,  2, 34, 10, 42},
                {48, 16, 56, 24, 50, 18, 58, 26},
                {12, 44,  4, 36, 14, 46,  6, 38},
                {60, 28, 52, 20, 62, 30, 54, 22},
                { 3, 35, 11, 43,  1, 33,  9, 41},
                {51, 19, 59, 27, 49, 17, 57, 25},
                {15, 47,  7, 39, 13, 45,  5, 37},
                {63, 31, 55, 23, 61, 29, 53, 21}
            };
            
            float GetMatrix(uint x, uint y){
                int n = 8;
                return M8[x % n][y % n] / n*n;
            }

            float MapRange(float input, float inStart, float inEnd, float outStart, float outEnd){
                float slope = (outEnd - outStart) / (inEnd - inStart);
                float output = outStart + slope * (input - inStart);
                return output;
            }

            float3 getCol(float3 curcol, float val, float3 defCol){
                if (val > 0.5){
                    return defCol;
                }
                return _BackgroundCol;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float d = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv).r);
                float4 startCol = tex2D(_MainTex, i.uv);
                float4 col = startCol;

                float falloff = (1 - MapRange(d, 0, 1, _StartRange, 1/_EndRange));

                uint2 pixel = i.uv * _ScreenParams.xy;
                float threshold = GetMatrix(pixel.x, pixel.y)/ 64 - 1/2;

                float val = falloff + threshold;


                float mult = round(val);

                if (mult < 0.5){
                    // col = _BackgroundCol;
                }
                col.rgb = col.rgb;

                col.rgb = getCol(col.rgb, falloff * 0.55 + threshold/2, startCol);
                col.a = 1.0;

                // col.rgb = mult * col.rgb;

                // return fixed4(falloff, falloff, falloff, 1.0);

                return col;
            }
            ENDCG
        }
    }
}
