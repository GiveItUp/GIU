Shader "Impossiball/PlatformTop"
{

	Properties
	{
		_Color ("Inner Color", Color) = (1.0,1.0,1.0,1)
    	_MainTex ("Texture", 2D) = "white" { }
    	_Sat ("Saturation", float) = 1
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent-1"
		}
	
    	Pass
    	{
    	
    		Blend SrcAlpha OneMinusSrcAlpha
    		ZWrite Off
			ZTest Less
    
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _Color;
			float _Sat;
			
			
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
			    o.color = _Color * _Sat;
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