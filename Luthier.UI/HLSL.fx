/************* DATA STRUCTS **************/
/*
HLSL packing rules are similar to performing a #pragma pack 4 with Visual Studio, which packs data into 4-byte boundaries. Additionally, 
HLSL packs data so that it does not cross a 16-byte boundary. Variables are packed into a given four-component vector until the variable 
will straddle a 4-vector boundary; the next variables will be bounced to the next four-component vector.
https://docs.microsoft.com/en-us/windows/desktop/direct3dhlsl/dx-graphics-hlsl-packing-rules
*/

cbuffer data : register(b0)
{
    float4x4 World;
    float4x4 WorldInverseTranspose;
    float4x4 ViewInverse;
    float4x4 WorldViewProj;
    float4 LightDirection;

    float3 SurfaceColor;
    float Kd;
    float3 LampColor;
    float SpecExpon;
    float3 AmbiColor;
    float Kr;
};

//cbuffer data :register(b0)
//{
//	float4x4 world;
//	float4x4 worldViewProj;
//	float4 lightDirection;
//};

struct VS_IN
{
	float4 position : POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
};

struct PS_IN
{
	float4 position : SV_POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
};

PS_IN VS( VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.position = mul(WorldViewProj,input.position);
	output.normal=mul(World,input.normal);
	output.color=input.color;

	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	//return input.color;
	return saturate(-dot(normalize(input.normal),normalize(LightDirection)*0.3 + 0.7)*input.color);
}