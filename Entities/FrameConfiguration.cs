using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limestone.Entities
{
    public delegate void FrameAction(Entity entity, FrameConfiguration configuration);

    public class FrameConfiguration
    {
        FrameAction frameAction;

        internal List<FrameCollection> frameCollections = new List<FrameCollection>();
        public Frame currentFrame;

        public Entity parent;

        public FrameConfiguration() { }

        public FrameConfiguration(FrameAction frameAction, Entity parent, params FrameCollection[] frameCollections)
        {
            this.frameAction = frameAction;

            this.parent = parent;

            this.frameCollections.AddRange(frameCollections);

            currentFrame = frameCollections[0].currentFFrame;

            frameCollections[0].active = true;
        }

        internal void SetFrame(int frameCollectionIndex)
        {
            foreach (FrameCollection fc in frameCollections)
                fc.SetInactive();

            frameCollections[frameCollectionIndex].active = true;
            currentFrame = frameCollections[frameCollectionIndex].currentFFrame;
        }

        public void Update()
        {
            frameAction?.Invoke(parent, this);
        }

        /// <summary>
        /// Uses 2 Collections.
        /// First collection is the active;
        /// Second collection is the inactive; do SetFrame(1);
        /// See: Entity.SetFrame
        /// </summary>
        public static void FrameActionPresetactive1inactive2(Entity entity, FrameConfiguration configuration)
        {
            FrameCollection three = configuration.frameCollections[0];
            FrameCollection inactive2 = configuration.frameCollections[1];

            if (entity.moving)
            {
                if (!inactive2.active)
                {
                    three.Update();
                    configuration.currentFrame = three.currentFFrame;
                }
            }
            else
            {
                if (!inactive2.active)
                    configuration.currentFrame = three.frames[0];
            }

            if (inactive2.active)
            {
                inactive2.Update();
                configuration.currentFrame = inactive2.currentFFrame;
            }
        }

        /// <summary>
        /// Uses 1 Collection.
        /// Collection 1 is always active.
        /// </summary>
        public static void FrameActionPreset3(Entity entity, FrameConfiguration configuration)
        {
            FrameCollection walkFrames = configuration.frameCollections[0];

            walkFrames.Update();
            configuration.currentFrame = walkFrames.currentFFrame;
        }
    }
}
