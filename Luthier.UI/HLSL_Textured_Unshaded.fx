/*
HLSL packing rules are similar to performing a #pragma pack 4 with Visual Studio, which packs data into 4-byte boundaries. Additionally, 
HLSL packs data so that it does not cross a 16-byte boundary. Variables are packed into a given four-component vector until the variable 
will straddle a 4-vector boundary; the next variables will be bounced to the next four-component vector.
https://docs.microsoft.com/en-us/windows/desktop/direct3dhlsl/dx-graphics-hlsl-packing-rules
*/
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


/* data from application vertex buffer */
struct VS_IN
{
    float3 Position : POSITION;
    float3 Normal : NORMAL;
    float3 Tangent : TANGENT;
    float3 Binormal : BINORMAL;
    float2 UV : TEXCOORD;
};


struct PS_IN
{
    float4 position : SV_POSITION;
    float2 texcoord : TEXCOORD;
};

//texture
Texture2D textureMap;
SamplerState textureSampler
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

PS_IN VS(VS_IN input)
{
    PS_IN output = (PS_IN) 0;

    output.position = mul(WorldViewProj, float4(input.Position, 1));
    output.texcoord = input.UV;

    return output;
}

float4 PS(PS_IN input) : SV_Target
{
    //return float4(1, 1, 1, 1);
    float4 color = textureMap.Sample(textureSampler, input.texcoord);

    return float4(color.x, color.y, color.z, 1.0);
}