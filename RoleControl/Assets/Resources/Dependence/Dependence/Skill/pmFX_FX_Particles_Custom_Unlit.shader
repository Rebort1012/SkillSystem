// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "pmFX/FX/FX_Particles_Custom_Unlit"
{
	Properties
	{
		[HDR]_ParticleColor("Particle Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		_Emissive("Emissive", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_Src("Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 10
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
		_DepthDistance1("Depth Distance", Float) = 1
		_PowerScale("Power Scale", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite Off
		ZTest LEqual
		Blend [_Src] [_Dst]
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 5.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _CullMode;
		uniform float _Src;
		uniform float _Dst;
		uniform float _Emissive;
		uniform float4 _ParticleColor;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _PowerScale;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
			float4 temp_cast_0 = (_PowerScale).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth17 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth17 = saturate( abs( ( screenDepth17 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance1 ) ) );
			float temp_output_19_0 = saturate( ( ( tex2DNode1.a * i.vertexColor.a * _ParticleColor.a ) * distanceDepth17 ) );
			o.Emission = saturate( ( pow( ( ( _Emissive * ( _ParticleColor * tex2DNode1 ) ) * i.vertexColor ) , temp_cast_0 ) * temp_output_19_0 ) ).rgb;
			o.Alpha = temp_output_19_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18908
7;18;1906;1001;888.232;443.6279;1;True;False
Node;AmplifyShaderEditor.ColorNode;2;-427.9001,-186.6599;Inherit;False;Property;_ParticleColor;Particle Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2,2,2,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-499.459,27.23388;Inherit;True;Property;_MainTex;Main Tex;2;0;Create;True;0;0;0;False;0;False;-1;None;07b32018fa635e34f86bf115dc0c9724;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;4;-394.3995,259.9923;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-569.915,497.1248;Inherit;False;Property;_DepthDistance1;Depth Distance;7;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-178.9943,-229.8603;Inherit;False;Property;_Emissive;Emissive;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-181.2413,-79.82404;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;17;-183.2806,475.3183;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-112.4939,279.3795;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-10.97741,-223.2068;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;153.1497,377.6961;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-9.179852,119.9184;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-160.232,22.3721;Inherit;False;Property;_PowerScale;Power Scale;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;420.7474,486.4513;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;20;88.76801,9.372101;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;174.9808,197.1498;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-913.382,55.32193;Inherit;False;Property;_CullMode;CullMode;6;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;271.256,58.32751;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-913.92,-187.6355;Inherit;False;Property;_Src;Src;4;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.BlendMode;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-919.6107,-55.80194;Inherit;False;Property;_Dst;Dst;5;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.BlendMode;True;0;False;10;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;610.1464,74.08307;Float;False;True;-1;7;ASEMaterialInspector;0;0;Unlit;pmFX/FX/FX_Particles_Custom_Unlit;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;3;1;True;9;10;True;10;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;True;12;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;17;0;16;0
WireConnection;5;0;1;4
WireConnection;5;1;4;4
WireConnection;5;2;2;4
WireConnection;8;0;7;0
WireConnection;8;1;3;0
WireConnection;18;0;5;0
WireConnection;18;1;17;0
WireConnection;6;0;8;0
WireConnection;6;1;4;0
WireConnection;19;0;18;0
WireConnection;20;0;6;0
WireConnection;20;1;21;0
WireConnection;11;0;20;0
WireConnection;11;1;19;0
WireConnection;15;0;11;0
WireConnection;0;2;15;0
WireConnection;0;9;19;0
ASEEND*/
//CHKSM=C443B359AFC9A7B6A82B4BB55CCD03DCF33C3400