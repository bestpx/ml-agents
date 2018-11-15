using MLAgents;
using UnityEngine;

namespace Test
{
    public class TestAcademy : Academy 
    {
        public override void AcademyReset()
        {
            base.AcademyReset();
            Debug.Log("Academy reset");
        }
    }
}