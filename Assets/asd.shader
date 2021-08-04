Shader "Custom/asd"
{
        Properties{
                //_Color("Main Color", Color) = (1,1,1,1)
                _MainTex("Base (RGB)", 2D) = "white" {}
                //_SecondTex("Second (RGB)", 2D) = "white" {}
                //_MixMask("Mask (R)", 2D) = "black" {}
        }
            SubShader{
                    /*Tags { "RenderType" = "Opaque" }
                    LOD 200*/

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            //sampler2D _SecondTex;
            //sampler2D _MixMask;
            //float4 _Color;

            struct Input {
                    float2 uv_MainTex;
                    //float2 uv_MixMask;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                    //half4 tex = tex2D(_MainTex, IN.uv_MainTex);
                    //half4 second = tex2D(_SecondTex, IN.uv_MainTex);
                    //half4 mask = tex2D(_MixMask, IN.uv_MixMask);

                //half4 c = lerp(tex, second, mask.r) * _Color;
                    //o.Albedo = c.rgb;
                    //o.Alpha = c.a;
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
            }
            ENDCG
        }

            //Fallback "VertexLit"
                Fallback "Diffuse"
    }
