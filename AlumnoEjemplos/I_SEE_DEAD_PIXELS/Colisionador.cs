using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using TgcViewer.Utils.TgcGeometry;
using TgcViewer;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class Colisionador:Malla
    {
        //public short estadoDinamico;
        public bool conCamara = false;
        public float radio;
        public Matrix matrizDeAlineacion= new Matrix();
        //public estructuraEsferaBoundingAsociada esferaBounding;
        //public struct estructuraEsferaBoundingAsociada //estructura de boundingSphere asociada al colisionador, por ahora un centro y un radio
        //{
        //    public Vector3 centro;
        //    public float radio;
        //}
        public struct EstructVertice //estructura usada para almacenar c/u de los elementos levantados del Vertex Buffer
        {
            public Vector3 p;
            public Vector3 n;
            public float tu, tv;
            public static readonly VertexFormats Formato = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture1;
        }
        public void crearMatrizDeAlineacion(Vector3 eje)
        {
            float hip = (float)Math.Sqrt(eje.X * eje.X + eje.Z * eje.Z);
            matrizDeAlineacion.M11 = (eje.Z) / hip;
            matrizDeAlineacion.M13 = -(eje.X) / hip;
            matrizDeAlineacion.M14 = 0;
            matrizDeAlineacion.M21 = 0;
            matrizDeAlineacion.M22 = 1;
            matrizDeAlineacion.M23 = 0;
            matrizDeAlineacion.M24 = 0;
            matrizDeAlineacion.M31 = eje.X / hip;
            matrizDeAlineacion.M32 = 0;
            matrizDeAlineacion.M33 = (eje.Z) / hip;
            matrizDeAlineacion.M34 = 0;
            matrizDeAlineacion.M41 = 0;
            matrizDeAlineacion.M42 = 0;
            matrizDeAlineacion.M43 = 0;
            matrizDeAlineacion.M44 = 1;
        }
        //public void crearEsferaBoundingAsociada()
        //{
        //    Device dispositivo = GuiController.Instance.D3dDevice;
        //
        //    //variable donde almacenaré el vertex buffer
        //    VertexBuffer vertexBufferTemp = this.malla1.VertexBuffer;
        //    //lockeando el vertex buffer 
        //    GraphicsStream vertices = vertexBufferTemp.Lock(0, 0, LockFlags.None);
        //    float radio = Geometry.ComputeBoundingSphere(vertices, this.malla1.NumberVertices, this.malla1.VertexFormat, out this.esferaBounding.centro);
        //    this.esferaBounding.radio = radio;
        //    //deslockeando el buffer
        //    vertexBufferTemp.Unlock();
        //}
        public void inicializar()
        {
            this.inicializarGral();
            //this.crearEsferaBoundingAsociada();
        }
        public Colisionador(float masa, Vector3 posicion, String tipo, String rutaMesh, String textura)
        {
            Tipo = tipo;
            Masa = masa;
            PosicionActual = posicion;
            PosicionInicial = posicion;
            if (tipo == "archivoX")
            {
                RutaDeLaTextura = textura;
                RutaDelMesh = rutaMesh;
            }
            if (tipo == "archivoXML")
            {
                RutaDelMesh = rutaMesh;
            }
            this.inicializar();
        }
        public void renderizar() { this.renderizarGral();}
        public void renderizar(Vector3 eje,Vector3 velocidadAct) { this.renderizarGral(eje,this.matrizDeAlineacion,velocidadAct); }
    }
}
