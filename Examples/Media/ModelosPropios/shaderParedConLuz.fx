//Matrices de transformacion 
float4x4 matWorld;                                      //Matriz de transformacion World
float4x4 matWorldView;                                  //Matriz World * View
float4x4 matWorldViewProj :WORLDVIEWPROJECTION;         //Matriz World * View * Projection
float4x4 matInverseTransposeWorld;                      //Matriz Transpose(Invert(World))

texture textura_Base1;
sampler2D muestra1=sampler_state
{
   Texture= <textura_Base1>;
   MINFILTER=LINEAR;
   MAGFILTER=LINEAR;
   MIPFILTER=LINEAR;
};

int tipoPared;
float factor=0;
float4 vecDirLuz=float4 (0,100,200,0);
float4 luzAmbiente;
float4 luzDifusa;
float4 luzDifusa;
float matAmbiente;
float matDiffuse;
float matEspecular;
float4 posOjo;//={0,0,-10,0};//-100
float exponencial;

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION;
   float4 Color :    COLOR;
   float2 Texcoord : TEXCOORD0;
   float3 Normal: NORMAL;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position : POSITION;
   float4 Color : COLOR;
   float2 Texcoord : TEXCOORD0;
   float3 Pos: TEXCOORD1;
};


struct VS_OUTPUT2
{
   float4 position: POSITION;
   float2 texCoord: TEXCOORD0;
   float3 normal: TEXCOORD1;
   float3 view: TEXCOORD2;
};
//VS_OUTPUT2 vsBlinnPhong(VS_INPUT IN)
//{
//   VS_OUTPUT2 OUT;
//   OUT.position=mul(IN.Position,matWorldViewProj);
//   OUT.texCoord=IN.Texcoord;
//   OUT.normal=mul(IN.Normal,matWorldViewProj);//acá es donde iba lo comentado arriba
//   float4 posOjo2=mul(posOjo,matWorldViewProj);
//   float4 worldPos=mul(IN.Position,matWorldViewProj);//algo parecido a lo de la normal acá supestamente llevaba el pto al matWorld
//   OUT.view= posOjo.xyz-worldPos.xyz;
//   return OUT;
//}

//float4 psBlinnPhong(VS_OUTPUT2 IN):COLOR
//{
//   float3 light=normalize(-vecDirLuz.xyz);
//   float3 view=normalize(IN.view);
//   float3 normal=normalize(IN.normal);
//   float3 halfway=normalize(-light+view);
//   float3 ambient= matAmbiente*luzAmbiente.xyz;
   //float3 diffuse= saturate(dot(normal,light))*matDiffuse; //calculando la difusa
//   float s=max(dot(vecDirLuz,normal),0.0f);
//   float3 diffuse= s*luzDifusa.rgb*matDiffuse; 
   //float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;
//   float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;
//   float2 texcoord=IN.texCoord;
//   float4 texColor=tex2D(muestra1,texcoord);
   //float3 color=(saturate(ambient+diffuse)*texColor+specular)+luzDifusa.rgb; 
//   float3 base={0.5,0.5,0.5};
//   float3 color=(saturate(ambient+diffuse)*texColor+specular)*luzDifusa+0.15*texColor;

//   float alpha=matDiffuse*texColor.a;//float alpha=matDiffuse.a*texColor.a;
   //return float4(diffuse,alpha); usado para probar solamente la difusa
//   return float4(IN.texCoord,0);//float4(color,alpha);
   //return float4(specular+ambient,alpha);//usado para probar solamente la especular
//}

VS_OUTPUT sombraVS (VS_INPUT input)
{
   VS_OUTPUT output1=(VS_OUTPUT)0;
   output1.Color=float4(0.2,0.2,0.2,0);             
   output1.Position=mul(input.Position,matWorldViewProj);
   output1.Texcoord=input.Texcoord;
   output1.Pos=float3(input.Position.x,input.Position.y,input.Position.z);
   return output1;   
};

void psSombra (float4 color0: COLOR,float2 textureCoords: TEXCOORD0,float3 posicion: TEXCOORD1 ,out float4 color:COLOR)
{
   if(tipoPared==1) factor=abs(posicion.x);
   if(tipoPared==2) factor=abs(posicion.z);    
   if(tipoPared==3) factor=abs(posicion.x);   
   if(tipoPared==4) factor=abs(posicion.z);  
    
   float4 color1=tex2D(muestra1,textureCoords);
   color=(factor*0.18)*color0+(1-factor*0.18)*color1;
};

float4 psComun (float4 c: COLOR0,float2 tex0:TEXCOORD):COLOR
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   float3 difuso=c.rgb*texColor;
   return float4 (difuso,c.a);
};

//technique BlinnPhong
//{
//   pass Pass_0
//   {
//      VertexShader=compile vs_2_0 vsBlinnPhong();
//      PixelShader =compile ps_2_0 psBlinnPhong();
//   }
//};  

technique Sombrear
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 sombraVS();
         PixelShader = compile ps_2_0 psSombra();
      }
}
