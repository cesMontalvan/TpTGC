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

using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;

namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class DeDisparo:Armas
    {
        public Malla modelo;
        public override void volverAInicio()
        {
            modelo.PosicionActual = new Vector3(GuiController.Instance.FpsCamera.Position.X, -25.5f, GuiController.Instance.FpsCamera.Position.Z);

        }
        public void sonar()
        {
            //Device dispositivo=null;
            //SecondaryBuffer sonido = null;
            //dispositivo.SetCooperativeLevel((this, CooperativeLevel.Normal);
            //sonido = new SecondaryBuffer(GuiController.Instance.ExamplesMediaDir + "ModelosPropios\\shader22.fx",dispositivo);

 
        }
    }
}
