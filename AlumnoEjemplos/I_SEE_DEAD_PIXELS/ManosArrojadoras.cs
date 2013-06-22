using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
    public class ManosArrojadoras:Armas
    {
        public List<Colisionador> arrojables = new List<Colisionador>();
        public Colisionador arrojableCargado;

        public void inicializar()
        {
            Colisionador coli1 = new Colisionador((float)GuiController.Instance.Modifiers["masa"], new Vector3(-40, -23, -85), "archivoX", "ModelosPropios\\colisionadorEsferaRadio2-16segs.X", "ModelosPropios\\textura-plata.bmp ");
            coli1.radio = 2.0f;
            //coli1.estadoDinamico = 0;
            Colisionador coli2 = new Colisionador((float)GuiController.Instance.Modifiers["masa"], new Vector3(-20, -21, -85), "archivoX", "ModelosPropios\\colisionadorEsferaRadio4-16Segs.X", "ModelosPropios\\textured--texture--platinum--silver1.bmp");
            coli2.radio = 4.0f;
            //coli2.estadoDinamico = 0;
            Colisionador coli3 = new Colisionador((float)GuiController.Instance.Modifiers["masa"], new Vector3(0, -19, -85), "archivoX", "ModelosPropios\\colisionadorEsferaRadio618Segs.X", "ModelosPropios\\textura-plata1.bmp");
            coli3.radio = 6.0f;
            //coli3.estadoDinamico = 0;
            Colisionador coli4 = new Colisionador((float)GuiController.Instance.Modifiers["masa"], new Vector3(20, -17, -85), "archivoX", "ModelosPropios\\colisionadorEsferaRadio8-40Segs.X", "ModelosPropios\\textura-plata2.bmp");
            coli4.radio = 8.0f;
            //coli4.estadoDinamico = 0;
            Colisionador coli5 = new Colisionador((float)GuiController.Instance.Modifiers["masa"], new Vector3(40, -15, -85), "archivoX", "ModelosPropios\\colisionadorEsferaRadio10-18Segs.X", "ModelosPropios\\texturaMetalRugosoJoLL.bmp");
            coli5.radio = 10.0f;
            //coli5.estadoDinamico = 0;
            this.arrojables.Add(coli1);
            this.arrojables.Add(coli2);
            this.arrojables.Add(coli3);
            this.arrojables.Add(coli4);
            this.arrojables.Add(coli5);
        }
        public override void volverAInicio()
        {
            throw new NotImplementedException();
        }
    }
}
