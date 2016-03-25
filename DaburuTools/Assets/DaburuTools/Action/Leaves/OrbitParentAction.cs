//using UnityEngine;
//using System.Collections;
//
//namespace DaburuTools
//{
//	namespace Action
//	{
//		public class OrbitParentAction : Action
//		{
//			Transform mTransform;
//			Transform mParentTransform;
//			Vector3 mvecAxisOfRotation;
//			float mfStartingRadians;
//			float mfDeltaRadians;
//			float mfDistFromParent;
//			float mfOrbitSpeed;
//			float mfActionDuration;
//			float mfElaspedDuration;
//
//			public OrbitParentAction(Transform _transform, Transform _parentTransform)
//			{
//				mTransform = _transform;
//				mParentTransform = _parentTransform;
//				SetupAction();
//			}
//			public OrbitParentAction(Transform _transform, Transform _parentTransform,
//									 float _distFromParent, float _orbitSpeed, float _actionDuration)
//			{
//				mTransform = _transform;
//				mParentTransform = _parentTransform;
//				SetAction(_distFromParent, _orbitSpeed, _actionDuration);
//				SetupAction();
//			}
//
//			public void SetAction(float _distFromParent, float _orbitSpeed, float _actionDuration)
//			{
//				mfDistFromParent = _distFromParent;
//				mfOrbitSpeed = _orbitSpeed;
//				mfActionDuration = _actionDuration;
//			}
//			private void SetupAction()
//			{
//				
//				mfElaspedDuration = 0f;
//				mfDeltaRadians = 0f;
//			}
//			private float OrbitOffsetToDegree(Vector3 _offsetVec)
//			{
//				Mathf.Asin(
//			}
//			protected override void OnActionBegin()
//			{
//				base.OnActionBegin();
//
//				SetupAction(); 
//			}
//
//
//
//			public override void RunAction()
//			{
//				base.RunAction();
//
//				mfElaspedDuration += Time.deltaTime;
//
//				// Remove self after action is finished.
//				if (mfElaspedDuration > mfActionDuration)
//				{
//					OnActionEnd();
//					mParent.Remove(this);
//				}
//			}
//			public override void MakeResettable(bool _bIsResettable)
//			{
//				base.MakeResettable(_bIsResettable);
//			}
//			public override void Reset()
//			{
//				SetupAction();
//			}
//		}
//	}
//}
