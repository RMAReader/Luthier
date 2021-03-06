﻿
/************* DATA STRUCTS **************/
cbuffer data : register(b0)
{
    float4x4 World;
    float4x4 WorldInverseTranspose;
    float4x4 WorldView;
    float4x4 WorldViewProj;
    float4 LightDirection;

    float3 AmbientColor;
    float AmbientCoefficient;
    float3 DiffuseColor;
    float DiffuseCoefficient;
    float3 SpecularColor;
    float SpecularCoefficient;
    float3 Padding1;
    float ShininessCoefficient;
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
	return input.color;
}