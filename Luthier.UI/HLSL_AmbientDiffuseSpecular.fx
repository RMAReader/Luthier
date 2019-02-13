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

/* data passed from vertex shader to pixel shader */
struct PS_IN
{
    float4 HPosition : SV_POSITION;
    float3 LightVec : TEXCOORD1;
    float3 WorldPosition : POSITION;
    float3 WorldNormal : NORMAL;
};


/*********** Generic Vertex Shader ******/

PS_IN VS(VS_IN IN)
{
	
    PS_IN OUT = (PS_IN) 0;

    OUT.WorldPosition = mul(WorldView, float4(IN.Position, 1)).xyz;
    
    OUT.WorldNormal = mul(WorldView, IN.Normal).xyz;
    
    OUT.LightVec = mul(WorldView, LightDirection).xyz;
        
    OUT.HPosition = mul(WorldViewProj, float4(IN.Position, 1));
	
    return OUT;
}



/********* pixel shaders ********/

float4 PS(PS_IN IN) : SV_Target
{
    float3 diffuseColor;
    float3 specularColor;
    float3 ambientColour = AmbientColor * AmbientCoefficient;
    
    float3 toLight = normalize(IN.LightVec);
    float3 vertexNormal = normalize(IN.WorldNormal);

    // Calculate the cosine of the angle between the vertex's normal vector
    // and the vector going to the light.
    float cosAngle = clamp(dot(vertexNormal, toLight), 0, 1);

    // Scale the color of this fragment based on its angle to the light.
    diffuseColor = DiffuseColor * cosAngle * DiffuseCoefficient;

    //Calculate the reflection vector
    float3 reflVect = normalize(reflect(vertexNormal, toLight));
    
    // Calculate a vector from the fragment location to the camera.
    // The camera is at the origin, so negating the vertex location gives the vector
    float3 toCamera = -normalize(IN.WorldPosition);

    // Calculate the cosine of the angle between the reflection vector
    // and the vector going to the camera.
    float cosAngle2 = clamp(dot(reflVect, toCamera), 0, 1);
    cosAngle2 = pow(cosAngle2, ShininessCoefficient);

    // The specular color is from the light source, not the object
    if (cosAngle > 0.0 && cosAngle2 > 0.0)
    {
        specularColor = SpecularColor * cosAngle2 * SpecularCoefficient;
        diffuseColor = diffuseColor * (1.0 - cosAngle2);
    }
    else
    {
        specularColor = float3(0.0, 0.0, 0.0);
    }
    
    return float4(ambientColour + diffuseColor + specularColor, 1);

}

