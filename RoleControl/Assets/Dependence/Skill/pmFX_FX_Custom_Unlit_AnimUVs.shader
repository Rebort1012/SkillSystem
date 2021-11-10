// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "pmFX/FX/FX_Custom_Unlit_animUVLayers"
{
	Properties
	{
		_Emissive("Emissive", Float) = 1
		_TexturePower("Texture Power", Float) = 1
		_MaskOpacity("Mask Opacity", Float) = 1
		_TextureTiling("Texture Tiling", Vector) = (1,1,1,1)
		_AlphaTiling("Alpha Tiling", Vector) = (1,1,1,1)
		[Toggle(_2NDTEXTURE_ON)] _2ndTexture("2nd Texture", Float) = 0
		_MainTex("Main Tex", 2D) = "white" {}
		_DepthDistance("Depth Distance", Float) = 1
		_TextureSpeed("Texture Speed", Vector) = (0,0,0,0)
		_AlphaSpeed("Alpha Speed", Vector) = (0,0,0,0)
		_Mask("Mask", 2D) = "white" {}
		_MaskPower("Mask Power", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_Src("Src", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 10
		_AlphaTexture("Alpha Texture", 2D) = "white" {}
		_AlphaPower("Alpha Power", Float) = 0.2
		[Toggle(_2NDALPHA_ON)] _2ndAlpha("2nd Alpha", Float) = 0
		_ClipValue("Clip Value", Range( 0 , 1)) = 1
		[Toggle(_USEEROSION_ON)] _UseErosion("Use Erosion", Float) = 0
		_Min("Min", Float) = 0
		_Max("Max", Float) = 1
		_TintColor("Tint Color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite Off
		ZTest LEqual
		Blend [_Src] [_Dst]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 5.0
		#pragma shader_feature_local _2NDTEXTURE_ON
		#pragma shader_feature_local _USEEROSION_ON
		#pragma shader_feature_local _2NDALPHA_ON
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_tex4coord;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _Dst;
		uniform float _CullMode;
		uniform float _Src;
		uniform float _Emissive;
		uniform sampler2D _MainTex;
		uniform float4 _TextureSpeed;
		uniform float4 _TextureTiling;
		uniform float4 _TintColor;
		uniform float _TexturePower;
		uniform float _MaskOpacity;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _MaskPower;
		uniform sampler2D _AlphaTexture;
		uniform float4 _AlphaSpeed;
		uniform float4 _AlphaTiling;
		uniform float _AlphaPower;
		uniform float _Min;
		uniform float _Max;
		uniform float _ClipValue;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 TexCoordWT127 = (i.uv_tex4coord).zw;
			float2 uv_TexCoord94 = i.uv_texcoord * (_TextureTiling).xy + TexCoordWT127;
			float2 panner96 = ( 1.0 * _Time.y * (_TextureSpeed).xy + uv_TexCoord94);
			float4 tex2DNode72 = tex2D( _MainTex, panner96 );
			float2 uv_TexCoord98 = i.uv_texcoord * (_TextureTiling).zw + TexCoordWT127;
			float2 panner102 = ( 1.0 * _Time.y * (_TextureSpeed).zw + uv_TexCoord98);
			float4 tex2DNode90 = tex2D( _MainTex, panner102 );
			#ifdef _2NDTEXTURE_ON
				float4 staticSwitch89 = tex2DNode90;
			#else
				float4 staticSwitch89 = float4( 0,0,0,0 );
			#endif
			float4 saferPower73 = max( ( ( tex2DNode72 + staticSwitch89 ) * _TintColor ) , 0.0001 );
			float4 temp_cast_0 = (_TexturePower).xxxx;
			float4 MainTex87 = saturate( pow( saferPower73 , temp_cast_0 ) );
			float4 VertexColor78 = i.vertexColor;
			float VertexAlpha79 = i.vertexColor.a;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float saferPower135 = max( tex2D( _Mask, uv_Mask ).r , 0.0001 );
			float Opacity140 = ( _MaskOpacity * saturate( pow( saferPower135 , _MaskPower ) ) );
			float2 uv_TexCoord118 = i.uv_texcoord * (_AlphaTiling).xy + TexCoordWT127;
			float2 panner122 = ( 1.0 * _Time.y * (_AlphaSpeed).xy + uv_TexCoord118);
			float4 tex2DNode145 = tex2D( _AlphaTexture, panner122 );
			float2 uv_TexCoord119 = i.uv_texcoord * (_AlphaTiling).zw + TexCoordWT127;
			float2 panner121 = ( 1.0 * _Time.y * (_AlphaSpeed).zw + uv_TexCoord119);
			#ifdef _2NDALPHA_ON
				float staticSwitch148 = ( tex2DNode145.r * tex2D( _AlphaTexture, panner121 ).r );
			#else
				float staticSwitch148 = tex2DNode145.r;
			#endif
			float AlphaTexture150 = pow( staticSwitch148 , _AlphaPower );
			float smoothstepResult164 = smoothstep( _Min , _Max , step( ( 1.0 - VertexAlpha79 ) , ( AlphaTexture150 * _ClipValue ) ));
			#ifdef _USEEROSION_ON
				float staticSwitch166 = ( AlphaTexture150 * smoothstepResult164 );
			#else
				float staticSwitch166 = AlphaTexture150;
			#endif
			float clampResult169 = clamp( staticSwitch166 , 0.0 , 1.0 );
			float Erosion167 = clampResult169;
			float clampResult52 = clamp( ( VertexAlpha79 * Opacity140 * Erosion167 * ( tex2DNode72.a + tex2DNode90.a ) ) , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth184 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth184 = saturate( abs( ( screenDepth184 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) ) );
			float temp_output_185_0 = ( clampResult52 * distanceDepth184 );
			float4 clampResult186 = clamp( ( ( _Emissive * ( MainTex87 * VertexColor78 ) ) * temp_output_185_0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Emission = clampResult186.rgb;
			o.Alpha = temp_output_185_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18908
7;12;1906;1007;1577.006;900.4279;1.128709;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;125;-2561.386,-1467.317;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;126;-2352.251,-1472.375;Inherit;False;False;False;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-2099.977,-1473.253;Inherit;False;TexCoordWT;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;105;-2561.667,-32.18658;Inherit;False;Property;_AlphaTiling;Alpha Tiling;5;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;153;-2371.97,-30.32592;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;116;-2362.632,58.53553;Inherit;False;127;TexCoordWT;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;152;-2373.49,164.0509;Inherit;False;False;False;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;113;-1913.821,56.1031;Inherit;False;Property;_AlphaSpeed;Alpha Speed;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6,0,-0.4,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;118;-2136.674,-49.4743;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;117;-1719.696,161.4994;Inherit;False;False;False;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;119;-2130.711,144.9442;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;120;-1727.594,-29.13;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;121;-1436.424,141.4429;Inherit;False;3;0;FLOAT2;-1,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;122;-1437.235,-48.54181;Inherit;False;3;0;FLOAT2;-1,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;145;-1221.332,-76.32104;Inherit;True;Property;_AlphaTexture;Alpha Texture;16;0;Create;True;0;0;0;False;0;False;-1;None;bac6e5be1c423ed41b9b710f74004c80;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;144;-1222.088,112.6188;Inherit;True;Property;_TextureSample2;Texture Sample 2;16;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;145;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-870.9647,116.9098;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;148;-701.4106,-53.36597;Inherit;False;Property;_2ndAlpha;2nd Alpha;18;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-638.0928,66.9729;Inherit;False;Property;_AlphaPower;Alpha Power;17;0;Create;True;0;0;0;False;0;False;0.2;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;33;-1793.317,-1471.897;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;149;-446.5378,-47.95728;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;108;-2559.37,-795.0959;Inherit;False;Property;_TextureTiling;Texture Tiling;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;150;-269.4756,-51.92896;Inherit;False;AlphaTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-1588.036,-1380.26;Inherit;False;VertexAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;132;-2299.227,-584.71;Inherit;False;False;False;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;93;-1852.08,-701.3062;Inherit;False;Property;_TextureSpeed;Texture Speed;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;154;-2294.57,-691.7692;Inherit;False;127;TexCoordWT;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;158;16.51455,143.8722;Inherit;False;Property;_ClipValue;Clip Value;19;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;171;105.9916,-79.1087;Inherit;False;79;VertexAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;99.94761,28.1193;Inherit;False;150;AlphaTexture;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;98;-2073.943,-603.8632;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;131;-2302.12,-795.3333;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;99;-1629.319,-579.7558;Inherit;False;False;False;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;352.9776,125.3121;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;159;349.5354,-74.63458;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-2080.692,-814.9222;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;95;-1634.052,-795.6864;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;102;-1360.838,-599.5076;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;161;561.0347,-73.56281;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;96;-1360.5,-814.2854;Inherit;False;3;0;FLOAT2;-1,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;90;-1136.687,-628.8009;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;72;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;163;530.4707,88.65005;Inherit;False;Property;_Min;Min;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;531.1697,171.549;Inherit;False;Property;_Max;Max;22;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-1138.274,-843.5503;Inherit;True;Property;_MainTex;Main Tex;7;0;Create;True;0;0;0;False;0;False;-1;None;da5212a6a38c6a1428b38c7da572b599;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;89;-805.572,-651.5842;Inherit;False;Property;_2ndTexture;2nd Texture;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-2428.046,879.2521;Inherit;False;Property;_MaskPower;Mask Power;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;133;-2558.251,675.897;Inherit;True;Property;_Mask;Mask;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;164;706.9706,-72.25079;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;895.1047,34.60516;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;180;-560.4595,-622.3079;Inherit;False;Property;_TintColor;Tint Color;23;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-522.5885,-834.1927;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;135;-2205.613,704.52;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-318.4757,-645.8691;Inherit;False;Property;_TexturePower;Texture Power;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;166;1070.073,30.04118;Inherit;False;Property;_UseErosion;Use Erosion;20;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;137;-2005.282,786.2502;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2037.897,700.3394;Inherit;False;Property;_MaskOpacity;Mask Opacity;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-339.7197,-848.8524;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;169;1317.656,35.66131;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;73;-124.5773,-831.0436;Inherit;False;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-1845.804,704.9983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;140;-1686.126,700.462;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;157;52.88904,-831.2356;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;167;1486.542,29.93718;Inherit;False;Erosion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;182;-710.7449,-481.0377;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;1140.895,-573.0857;Inherit;False;167;Erosion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;1128.578,-710.7489;Inherit;False;79;VertexAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;1138.899,-647.3262;Inherit;False;140;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;209.7699,-835.5408;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-1589.61,-1476.572;Inherit;False;VertexColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;1132.97,-899.8678;Inherit;False;87;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;183;1749.386,-441.3607;Inherit;False;Property;_DepthDistance;Depth Distance;8;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;1139.426,-814.7252;Inherit;False;78;VertexColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;1510.772,-640.1938;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;1861.698,-1022.572;Inherit;False;Property;_Emissive;Emissive;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;1612.07,-906.1779;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;184;2136.021,-463.1672;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;52;1796.538,-695.8969;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;2054.895,-973.2057;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;2378.741,-619.506;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;2611.125,-822.9837;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;186;2805.689,-850.0007;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2392.928,-1980.795;Inherit;False;Property;_Src;Src;14;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-2247.491,-1982.847;Inherit;False;Property;_Dst;Dst;15;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2556.371,-1981.129;Inherit;False;Property;_CullMode;Cull Mode;13;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;176;3012.516,-929.6129;Float;False;True;-1;7;ASEMaterialInspector;0;0;Unlit;pmFX/FX/FX_Custom_Unlit_animUVLayers;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;1;0;True;43;0;True;45;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;True;76;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;139;-2557.417,513.6131;Inherit;False;1044.759;126.6325;;0;OPACITY MASK;0.3792801,0.1179245,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;82;-2558.33,-1662.745;Inherit;False;631.8994;126;XY;0;VERTEX TEXCOORD;1,0.08962262,0.227377,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;168;1.392479,-253.8328;Inherit;False;1662.093;126.3826;Alpha Erosion;0;;1,0.6598639,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;151;-2559.346,-250.4091;Inherit;False;2463.24;122.4647;Panning XY/ZW Alpha Texture;0;ALPHA TEXTURE;1,0.6631016,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;128;-1793.784,-1663.28;Inherit;False;380.8994;128.4304;Vertex Color;0;;1,0.08962262,0.227377,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;77;-2559.748,-1018.877;Inherit;False;2947.555;123.5361;Comment;0;MAIN TEXTURE;0.1933962,0.5077345,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;47;-2557.326,-2173.51;Inherit;False;430.9474;125.6107;Cull Mode/Blend Mode;0;ENUM;0.581415,1,0.2783019,1;0;0
WireConnection;126;0;125;0
WireConnection;127;0;126;0
WireConnection;153;0;105;0
WireConnection;152;0;105;0
WireConnection;118;0;153;0
WireConnection;118;1;116;0
WireConnection;117;0;113;0
WireConnection;119;0;152;0
WireConnection;119;1;116;0
WireConnection;120;0;113;0
WireConnection;121;0;119;0
WireConnection;121;2;117;0
WireConnection;122;0;118;0
WireConnection;122;2;120;0
WireConnection;145;1;122;0
WireConnection;144;1;121;0
WireConnection;146;0;145;1
WireConnection;146;1;144;1
WireConnection;148;1;145;1
WireConnection;148;0;146;0
WireConnection;149;0;148;0
WireConnection;149;1;147;0
WireConnection;150;0;149;0
WireConnection;79;0;33;4
WireConnection;132;0;108;0
WireConnection;98;0;132;0
WireConnection;98;1;154;0
WireConnection;131;0;108;0
WireConnection;99;0;93;0
WireConnection;160;0;170;0
WireConnection;160;1;158;0
WireConnection;159;0;171;0
WireConnection;94;0;131;0
WireConnection;94;1;154;0
WireConnection;95;0;93;0
WireConnection;102;0;98;0
WireConnection;102;2;99;0
WireConnection;161;0;159;0
WireConnection;161;1;160;0
WireConnection;96;0;94;0
WireConnection;96;2;95;0
WireConnection;90;1;102;0
WireConnection;72;1;96;0
WireConnection;89;0;90;0
WireConnection;164;0;161;0
WireConnection;164;1;163;0
WireConnection;164;2;162;0
WireConnection;165;0;170;0
WireConnection;165;1;164;0
WireConnection;88;0;72;0
WireConnection;88;1;89;0
WireConnection;135;0;133;1
WireConnection;135;1;134;0
WireConnection;166;1;170;0
WireConnection;166;0;165;0
WireConnection;137;0;135;0
WireConnection;181;0;88;0
WireConnection;181;1;180;0
WireConnection;169;0;166;0
WireConnection;73;0;181;0
WireConnection;73;1;74;0
WireConnection;138;0;37;0
WireConnection;138;1;137;0
WireConnection;140;0;138;0
WireConnection;157;0;73;0
WireConnection;167;0;169;0
WireConnection;182;0;72;4
WireConnection;182;1;90;4
WireConnection;87;0;157;0
WireConnection;78;0;33;0
WireConnection;177;0;80;0
WireConnection;177;1;141;0
WireConnection;177;2;172;0
WireConnection;177;3;182;0
WireConnection;35;0;86;0
WireConnection;35;1;81;0
WireConnection;184;0;183;0
WireConnection;52;0;177;0
WireConnection;42;0;39;0
WireConnection;42;1;35;0
WireConnection;185;0;52;0
WireConnection;185;1;184;0
WireConnection;178;0;42;0
WireConnection;178;1;185;0
WireConnection;186;0;178;0
WireConnection;176;2;186;0
WireConnection;176;9;185;0
ASEEND*/
//CHKSM=FD56F1D18E5A7554B75A52364E935302634BBAAB