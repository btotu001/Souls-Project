//this shader is used for x-ray feature
//shader renders an object 2 times (normal and x-ray version)

//shader name
Shader "Custom/xray_Shader"
{
    //shader properties
    Properties
    {
        //normal color and texture
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        //x-rayn color and texture
        _XrayColor("Xray Color", Color) = (1,1,1,1)
        _XrayTex("Xray TExture", 2D) = "white" {}

    }

    SubShader
    {
        //rendertype is opaque
        Tags { "RenderType"="Opaque" }
        
        //NORMAL cg
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        //normal texture
        sampler2D _MainTex;

        //texture input
        struct Input
        {
            float2 uv_MainTex;
        };

        //normal color
        fixed4 _Color;

        //surf function, outputs color values
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        //END NORMAL
        



        //START XRAY   
            // if objects depth is greater than other objects/ if it is behind an other object, we render it on top
            ZTEST GREATER
            //XRAY gc
            CGPROGRAM
            //in xray we dont use standard lighning or shadows, instead we create new unlit function below
            #pragma surface surf Unlit 
           

        //xray texture
        sampler2D _XrayTex;

        //texture input
        struct Input
        {
            float2 uv_MainTex;
        };

        // xray color
        fixed4 _XrayColor;

        //makes xray unlit with this function
        half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
        {
            half4 col;
            col.rgb = s.Albedo;
            col.a = s.Alpha;
            //returns the color value
            return col;
        }
       
        
        //outputs xray textures color values
        void surf(Input IN, inout SurfaceOutput o)
        {
            //takes xray texture and outputs its color value. color value is multiplied by xrays tintcolor
            fixed4 c = tex2D(_XrayTex, IN.uv_MainTex) * _XrayColor;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        //END XRAY
    }
    
}
