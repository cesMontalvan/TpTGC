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

float4 matAmbiente;
float4 luzAmbiente;
float4 matDiffuse;
float4 luzDifusa;
float4 matEspecular;
float4 luzEspecular;
float4 vecDirLuz;
float4 posOjo;
float exponencial;

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float3 Normal :   NORMAL;
   float4 Color :    COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float4 ColorDifuso : COLOR0;
   //float4 ColorEspecular : COLOR1;
   float2 Texcoord : TEXCOORD0;
};

VS_OUTPUT vertexShaderDifusa (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matInverseTransposeWorld);
   normalTransf=normalize(normalTransf);
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan y sus componentes van de 0 a 1 son%?
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   output1.ColorDifuso.rgb=s*(matDiffuse*luzDifusa).rgb;
   output1.ColorDifuso.a=matDiffuse.a;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   //output1.ColorDifuso=input1.Color;
   output1.Texcoord=input1.Texcoord;

   return output1;
};
VS_OUTPUT vertexShaderMasAmbient(VS_INPUT input1)
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
   output1.ColorDifuso.rgb=difusoAux+ambienteAux;
   output1.ColorDifuso.a=matDiffuse.a;

   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   
   output1.Texcoord=input1.Texcoord;

   return output1;
}

VS_OUTPUT vertexShaderDifuseAmbienteSpecular(VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;

   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matInverseTransposeWorld);
   normalTransf=normalize(normalTransf);

   //pasando la pos del vertice al espacio homogeneo
   float3 posVertice= mul(input1.Position,matWorldViewProj).xyz;
   
   //computando el vector que va desde el vertice que refleja a el ojo del observador/Camara
   float3 posOjoTransf= normalize(mul(posOjo,matWorldViewProj).xyz-posVertice);//(posOjo.xyz-posVertice);//
   
   //calculando el vector refleccion (rayo de luz que sale despues de rebotar en el vertice)
   float3 ref= reflect(-vecDirLuz,normalTransf);
   
   //determinando cuanta luz especular va al ojo
   float t=pow(max(dot(ref,posOjoTransf),0.0f),exponencial);

   //calculando la intensidad de luz difusa que el vertice rebota
   float s=max(dot(vecDirLuz,normalTransf),0.0f);

   //calculando las luces: especular, difusa, y ambiente
   float3 spec=t*(matEspecular*luzEspecular).rgb;
   float3 difusa=s*(matDiffuse*luzDifusa).rgb;
   float3 ambiente=(luzAmbiente*matAmbiente).rgb;
   
   output1.ColorDifuso.rgb= spec;//ambiente+difusa+spec;
   output1.ColorDifuso.a=matDiffuse.a;
   
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);

   output1.Texcoord=input1.Texcoord;

   return output1; 
}
  
 
float4 pixelShader (float4 c: COLOR0, float4 spec:COLOR1,float2 tex0:TEXCOORD):COLOR
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   float3 difuso=c.rgb*texColor;
   return float4 (difuso,c.a);//(difuso+spec.rgb,c.a);
}

technique RenderizarDifusa
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vertexShaderDifusa();
      PixelShader = compile ps_2_0 pixelShader();
   }
}
technique RenderizarDifusaYAmbient
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vertexShaderMasAmbient();
         PixelShader = compile ps_2_0 pixelShader();
      }
}
technique RenderizarLuzCompleta
{
   pass Pass_0
   {
      VertexShader=compile vs_2_0 vertexShaderDifuseAmbienteSpecular();
      PixelShader = compile ps_2_0 pixelShader();
   }
}