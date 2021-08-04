Shader "Custom/qwe"
{
    Properties
    {
        _MainTex ("Main texture", 2D) = "white" {}
        _MaskTex("Mask texture", 2D) = "black" {}
    }
    SubShader
    {

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            half2 uv_MaskTex;
            half2 uv_MainTex;
        };


        void surf (Input inp, inout SurfaceOutput outp)
        {
            fixed3 masks = tex2D(_MaskTex, inp.uv_MaskTex);
            fixed3 clr = tex2D(_MainTex, inp.uv_MainTex) * masks;
            outp.Albedo = clr;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
