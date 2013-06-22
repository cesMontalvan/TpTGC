using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Collision;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class Malla
    {
        Vector3 posicionInicial;
        public Vector3 PosicionInicial
        {
            get
            {
                return posicionInicial;
            }
            set
            {
                posicionInicial = value;
            }
        }
        public Vector3 posicionActual;
        public Vector3 PosicionActual
        {
            set { this.posicionActual = value; }
            get { return this.posicionActual; }
        }
        String tipo;
        public String Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
            }
        }
        String rutaDelMesh;
        public String RutaDelMesh
        {
            get
            {
                return rutaDelMesh;
            }
            set
            {
                rutaDelMesh = value;
            }
        }
        float masa;
        public float Masa
        {
            get { return masa; }
            set { masa = value; }
        }
        private ExtendedMaterial[] arrayMats;
        private Material[] meshMateriales;
        private Texture[] meshTexturas;
        public Mesh malla1;
        String rutaDeLaTextura;
        public String RutaDeLaTextura
        {
            get
            {
                return rutaDeLaTextura;
            }
            set
            {
                rutaDeLaTextura = value;
            }
        }
        public short[] bufferDeIndices;
        public CustomVertex.PositionNormalTextured[] bufferDeVertices;
        public Effect efecto1,efecto3;
       
        
        public void inicializarGral()
        {
            Device dispositivo1 = GuiController.Instance.D3dDevice;

            if (Tipo == "archivoX")
            {
                //cargando la malla desde un archivo .X exportado con 3dsmax, la siguiente linea devuelve también arrayMats
                malla1 = Mesh.FromFile(GuiController.Instance.ExamplesMediaDir + RutaDelMesh, MeshFlags.Managed, dispositivo1, out arrayMats);
                //Cargando los materiales, analizando c/u de los subsets del mesh
                if ((arrayMats != null) && (arrayMats.Length > 0))
                {
                    meshMateriales = new Material[arrayMats.Length];
                    meshTexturas = new Texture[arrayMats.Length];
                    // cargamos cada material y cada textura en los array creados atras
                    for (int i = 0; i < arrayMats.Length; i++)
                    {   // cargamos el material
                        meshMateriales[i] = arrayMats[i].Material3D;
                        // si hay textura tambien la cargamos
                        if ((arrayMats[i].TextureFilename != null) && (arrayMats[i].TextureFilename != string.Empty))
                        {   //tenemos textura, entonces cargaremos la textura con textureLoader
                            meshTexturas[i] = TextureLoader.FromFile(dispositivo1, GuiController.Instance.ExamplesMediaDir + rutaDeLaTextura);
                        }
                    }

                }
            }
            TgcMesh mallaDeTgc;
            if (Tipo == "archivoXML")
            {
                Device dispositivo = GuiController.Instance.D3dDevice;
                TgcScene escena;
                TgcSceneLoader cargador = new TgcSceneLoader();
                escena = cargador.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + RutaDelMesh);
                mallaDeTgc = escena.Meshes[0];
                this.malla1 = mallaDeTgc.D3dMesh;
            }
            efecto1 = Effect.FromFile(dispositivo1, GuiController.Instance.ExamplesMediaDir + "ModelosPropios\\shader5.fx", null, null, ShaderFlags.None, null);
        }

        public void renderizarGral()
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            for (int i = 0; i < meshMateriales.Length; i++)
            {
                dispositivo.Transform.World = Matrix.Translation(PosicionActual);
                dispositivo.Material = meshMateriales[i];
                dispositivo.SetTexture(0, meshTexturas[i]);
                malla1.DrawSubset(i);
            }
        }
        public void renderizarGral(Vector3 eje, Matrix matrizAlinear, Vector3 velocidad)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;

            if (this.RutaDelMesh == "ModelosPropios\\bala.X")
            {
                efecto1.SetValue("matWorldViewProj", matrizAlinear*Matrix.RotationAxis(new Vector3 (eje.X,0,eje.Z), -0.55f*velocidad.Y) * Matrix.Translation(PosicionActual) * dispositivo.Transform.View * dispositivo.Transform.Projection);
            }
            else
            {
                efecto1.SetValue("matWorldViewProj", Matrix.RotationAxis(eje, -0.35f*velocidad.Y) * Matrix.Translation(PosicionActual) * dispositivo.Transform.View * dispositivo.Transform.Projection);
            }
            float x = GuiController.Instance.CurrentCamera.getLookAt().X ;
            float y = GuiController.Instance.CurrentCamera.getLookAt().Y ;
            float z = GuiController.Instance.CurrentCamera.getLookAt().Z ;
            //float xl = GuiController.Instance.CurrentCamera.getLookAt().X;
            //float yl = GuiController.Instance.CurrentCamera.getLookAt().Y;
            //float zl = GuiController.Instance.CurrentCamera.getLookAt().Z;
            
             
            efecto1.SetValue("matInverseTransposeWorld", Matrix.Invert(Matrix.TransposeMatrix(dispositivo.Transform.World)));
            efecto1.SetValue("matWorld", dispositivo.Transform.World);
            efecto1.SetValue("matWorldView", dispositivo.Transform.World * dispositivo.Transform.View);
            efecto1.SetValue("vecDirLuz", new Vector4(-300,2500, -1000, 0));
            //efecto1.SetValue("posOjo", new Vector4(0, 0 ,-100, 0));//new Vector4(x,y,z,0));
            efecto1.SetValue("textura_Base1", this.meshTexturas[0]);
            efecto1.SetValue("luzEspecular", new Vector4(0.65f, 0.60f, 0.60f, 1));// pasarAVec(especular));
                      
            //efecto1.Technique = "RenderizarDifusa";
            if ((bool)GuiController.Instance.Modifiers["blinnPhong"])
            {
                efecto1.Technique = "BlinnPhong";
                efecto1.SetValue("luzAmbiente", new Vector4(0.48f, 0.50f, 0.54f, 1));
                efecto1.SetValue("matDiffuse", 6.0f); //7
                efecto1.SetValue("matAmbiente", 1.65f);
                efecto1.SetValue("exponencial", 12.0f);
                efecto1.SetValue("matEspecular", 01.40f);
                efecto1.SetValue("luzDifusa", new Vector4(0.68f, 0.68f, 0.68f, 1));
            }
            else
            {
                efecto1.SetValue("luzAmbiente", new Vector4(0.48f, 0.50f, 0.54f, 1));
                efecto1.SetValue("matEspecular", 04.0f);
                efecto1.SetValue("luzDifusa", new Vector4(0.0823f, 0.0823f, 0.0820f, 1)); 
                efecto1.SetValue("exponencial", 03.1f); efecto1.SetValue("matAmbiente", 01.65f);
                efecto1.SetValue("luzDifusa", new Vector4(0.68f, 0.68f, 0.68f, 1));
                if ((bool)GuiController.Instance.Modifiers["luzEspec"]) { efecto1.Technique = "RenderizarEspecularDifusaAmbiente"; efecto1.SetValue("matDiffuse", 0.015f);  }
                else
                {
                    efecto1.SetValue("matDiffuse", 7.15f);
                    if ((bool)GuiController.Instance.Modifiers["luzDif"] && (bool)GuiController.Instance.Modifiers["luzAmb"]) { efecto1.Technique = "RenderizarDifusaYAmbiente"; }
                    else
                    {
                        if (!(bool)GuiController.Instance.Modifiers["luzDif"] && (bool)GuiController.Instance.Modifiers["luzAmb"]) { efecto1.Technique = "RenderizarAmbiente"; }
                        else
                        {
                            if ((bool)GuiController.Instance.Modifiers["luzDif"] && !(bool)GuiController.Instance.Modifiers["luzAmb"]) { efecto1.Technique = "RenderizarDifusa"; }
                            else
                            {
                                if (!(bool)GuiController.Instance.Modifiers["luzDif"] && !(bool)GuiController.Instance.Modifiers["luzAmb"]) { efecto1.Technique = "RenderizarTextura"; }
                                else { }
                            }
                        }
                    }
                }
            }
            ////efecto3.SetValue("matWorldViewProjection", Matrix.RotationAxis(eje, acum) * Matrix.Translation(PosicionActual) * dispositivo.Transform.View * dispositivo.Transform.Projection);
            ////efecto3.SetValue("matWorld", dispositivo.Transform.World);
            ////efecto3.SetValue("vecDirLuz", new Vector4(0, 200, 0, 0));
            ////efecto3.SetValue("fvEyePosition", new Vector4(x, y + 2, z,0));//new Vector4(x,y,z,0));
            ////efecto3.SetValue("base_Tex", this.meshTexturas[0]);
            ////efecto3.SetValue("bump_Tex", textura1);
            ////efecto3.SetValue("matWorldViewProj", Matrix.RotationAxis(eje, acum) * Matrix.Translation(PosicionActual) * dispositivo.Transform.View * dispositivo.Transform.Projection);
            ////efecto3.SetValue("matTransposed", Matrix.TransposeMatrix(Matrix.Invert(dispositivo.Transform.World)));
            ////efecto3.SetValue("matEspecial", Matrix.RotationAxis(eje, -0.35f * velocidad.Y) * Matrix.Translation(PosicionActual) * dispositivo.Transform.World);
            ////efecto3.SetValue("matWorld", dispositivo.Transform.World);
            ////efecto3.SetValue("matWorldView", dispositivo.Transform.World * dispositivo.Transform.View);
            //efecto1.Technique="RenderizarAmbiente";

            int numPasadas = efecto1.Begin(0);
            for (int pas = 0; pas < numPasadas; pas++)
            {
                efecto1.BeginPass(pas);
                for (int i = 0; i < meshMateriales.Length; i++)
                {
                    dispositivo.Transform.World = Matrix.Translation(PosicionActual);
                    dispositivo.Material = meshMateriales[i];
                    dispositivo.SetTexture(0, meshTexturas[i]);
                    malla1.DrawSubset(i);
                    
                }
                efecto1.EndPass();
            }
            efecto1.End();
 
        }
    }
}
