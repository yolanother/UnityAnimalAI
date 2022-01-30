using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Utilities;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.NodeCanvas
{
    [Category("DoubTech/Animal AI/Movement")]
    [Description("Block movement in the controller")]
    public class StopMovement : ActionTask<IAgentState>
    {
        protected override void OnExecute()
        {
            base.OnExecute();
            agent.Stop = true;
            EndAction(true);
        }
    }
}
