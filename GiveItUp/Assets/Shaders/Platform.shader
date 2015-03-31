Shader "Impossiball/Platform"
{

	Properties
	{
    	_MainTex ("Texture", 2D) = "white" { }
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
	
    	Pass
    	{
    	
    		Blend SrcAlpha OneMinusSrcAlpha
    		ZWrite On
			ZTest Less
    
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			
			struct v2f {
			    half4  pos : SV_POSITION;
			    fixed2  uv : TEXCOORD0;
			    fixed4 color : COLOR;
			};
			
			fixed4 _MainTex_ST;
			
			v2f vert (appdata_full v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
			    fixed4 texcol = tex2D (_MainTex, i.uv);
              
			    return texcol;
			}
			
			ENDCG
    	}
	}

Fallback "VertexLit"
}