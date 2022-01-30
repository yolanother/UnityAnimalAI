using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Utilities;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.NodeCanvas
{
    [Category("DoubTech/Animal AI/Movement")]
    [Description("Allow movement in the controller")]
    public class AllowMovement : ActionTask<IAgentState>
    {
        protected override void OnExecute()
        {
            base.OnExecute();
            agent.Stop = false;
            EndAction(true);
        }
    }
}
