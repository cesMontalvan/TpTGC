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
float4 matAmbiente;
float4 luzAmbiente;
float4 matDiffuse;
float4 luzDifusa=float4 (0.8,0.8,0.8,0);

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

VS_OUTPUT sombraVS (VS_INPUT input)
{
   VS_OUTPUT output1=(VS_OUTPUT)0;
   output1.Color=float4(0.2,0.2,0.2,0);             
   output1.Position=mul(input.Position,matWorldViewProj);
   output1.Texcoord=input.Texcoord;
   output1.Pos=float3(input.Position.x,input.Position.y,input.Position.z);
   return output1;   
};

VS_OUTPUT vsIluminacion(VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;

   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matInverseTransposeWorld);
   normalTransf=normalize(normalTransf);
   
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan y sus componentes van de 0 a 1 son%?
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   float3 difusoAux = s*(matDiffuse*luzDifusa).rgb;
   float3 ambienteAux= (luzAmbiente*matAmbiente).rgb;

   output1.Color.rgb=difusoAux+ambienteAux;
   output1.Color.a=matDiffuse.a;

   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   
   output1.Texcoord=input1.Texcoord;
   output1.Pos=float3(input1.Position.x,input1.Position.y,input1.Position.z);

   return output1;
}

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

technique Iluminar
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vsIluminacion();
         PixelShader = compile ps_2_0 psComun();
      }
}

technique Sombrear
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 sombraVS();
         PixelShader = compile ps_2_0 psSombra();
      }
}
