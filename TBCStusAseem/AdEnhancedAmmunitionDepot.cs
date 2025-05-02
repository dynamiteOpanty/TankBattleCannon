using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using UnityEngine;
using Modding;
using Modding.Serialization;
using Modding.Modules;
using Modding.Blocks;
using skpCustomModule;
using Vector3 = UnityEngine.Vector3;

namespace TBCStusSpace
{
    public class AdEnhancedAmmunitionDepot : BlockScript
    {
        private BlockBehaviour bb;
        private AdShootingBehavour[] ChildObjects;
        private GameObject MachineGO;
        private bool explodeOK = true;
        public AudioSource audiosource;
        public AudioClip sound;
        private bool powerchange = true;
        public override void OnBlockPlaced()
        {
            base.OnBlockPlaced();
        }
        public override void OnSimulateStart()
        {
            base.OnSimulateStart();

            audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.spatialBlend = 1.0f;
            audiosource.volume = 1.5f;
            sound = ModResource.GetAudioClip("APHit");

            if(bb == null)
            {
                bb = this.gameObject.GetComponent<BlockBehaviour>();
            }
            var parent = this.transform;
            while(parent.name != "Simulation Machine")
            {
                parent = parent.parent;
            }
            MachineGO = parent.gameObject;
        }
        public override void SimulateFixedUpdateAlways()
        {
            base.SimulateFixedUpdateAlways();
            if(explodeOK)
            {
                if (bb.BlockHealth.health == 0f)
                {
                    var fireTag = this.gameObject.GetComponent<FireTag>();
                    if(fireTag)
                    {
                        fireTag.Ignite();
                    }
                    audiosource.PlayOneShot(sound);
                    explodeOK = false;
                }
                
            }
            if(powerchange)
            {
                //TODO 毎フレーム呼ぶ必要があるか？
                ChildObjects = MachineGO.GetComponentsInChildren<AdShootingBehavour>();
                foreach (AdShootingBehavour childobject in ChildObjects)
                {
                    childobject.PowerSlider.Value += 25;
                }
                powerchange = false;
            }
        }
    }
}
