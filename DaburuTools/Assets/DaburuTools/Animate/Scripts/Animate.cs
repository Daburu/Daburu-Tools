/* This is a modified version of the original class.
 * Credit goes to Chong Nian Kai for coming up with the original
 * basic version of this class. I merely took it and enhanced it
 * for our purposes.
 */

using UnityEngine;
using System.Collections;

// Animate.cs: Handles all transform positional and rotational animations
namespace DaburuTools
{
	namespace Animate
	{
		public class Animate
		{
			// Animation Definition Fields
			private float fExpandContract_Timer = 0f;               // fExpandContract_Timer: The time taken to complete the expand-contract sequence
			private int nExpandContract_Frequency = 0;              // nExpandContract_Frequency: The number of times the object pulses throughout the entire sequence
			private float fExpandContract_Size = 0f;                // fExpandContract_Size: The biggest size the object can get when it expands
			private Vector3 vExpandContract_InitialScale;           // vExpandContract_InitialScale: The initial scale of object
			private bool bExpandContract_IsOverridable = true;      // bExpandContract_IsOverridable: Determies if the current animation can be overriden
			private bool bExpandContract_IsLoopForever = false;		// bExpandContract_IsLoopForever: Determines if the  current animation is going to loop forever.

			private float fIdle_Speed = 0f;                         // fIdle_Speed: The speed at which the object is idling
			private float fIdle_Radius = 0f;                        // fIdle_Radius: The furthest distance at which the object can idle
			private Vector3 vIdle_InitialPosition;                  // vIdle_InitialPosition: The initial position of the when the method is called
			private Vector3 vIdle_TargetPosition;                   // vIdle_TargetPosition: The target position at which it is going to travel to
			private bool bIdle_IsOverridable = true;                // bIdle_IsOverridable: Determines if the current animation can be overriden
			private bool bIdle_RestrictXAxis = false;               // bIdle_RestrictXAxis: Set if the idling will not travel in the x-direction
			private bool bIdle_RestrictYAxis = false;               // bIdle_RestrictYAxis: Set if the idling will not travel in the y-direction
			private bool bIdle_IsComingToAHalt = false;             // bIdle_IsComingToAHalt: If this is true, the animation will be running for 'StopIdle' instead of 'Idle'

			private float fIdleRotation_MinRotation = 0f;           // fIdleRotation_MinRotation: The minimum amount of rotation to be applied in one turn
			private float fIdleRotation_RotationRange = 0f;         // fIdleRotation_RotationRange: The range of rotation between the minimum and the maximum of the rotation range
			private float fIdleRotation_MinTime = 0f;               // fIdleRotation_MinTime: The minimum amount of time to complete one turn
			private float fIdleRotation_TimeRange = 0f;             // fIdleRotation_TimeRange: The range of rotation between the minimum and maximum time of one turn
			private bool bIdleRotation_IsOverridable = true;        // bIdleRotation_IsOverridable: Determines if the current animation can be overriden
			private float fIdleRotation_CurrentTime = 0f;           // fIdleRotation_CurrentTime: The current time of the current turn, time will reset to 0 if its a new turn
			private float fIdleRotation_CurrentTotalTime = 0f;      // fIdleRotation_CurrentTotalTime: The current total time in that turn
			private float fIdleRotation_CurrentTotalRotation = 0f;  // fIdleRotation_CurrentTotalRotation: The current total rotation in that turn
			private bool bIdleRotation_SmoothTransition = false;    // bIdleRotation_SmoothTransition: Determines if the switching between turns transits nicely
			private float fIdleRotation_CurrentTorque = 0f;         // fIdleRotation_Torque: The current angular velocity of the object
			private Quaternion qIdleRotation_InitialQuaternion;     // qIdleRotation_InitialQuaternion: The initial rotation of the first animation
			private bool bIdleRotation_IsComingToAHalt = false;     // bIdleRotation_IsComingToAHalt: If this is true, the animation will be running for 'StopIdleRotation' instead of ''

			// Timing Fields
			private float fExpandContract_CurrentTimer = 0f;    // fExpandContract_CurrentTimer: The current time for the expand-contract sequence

			// Transform Reference
			private Transform mTransform;                       // _mTransform: The transform reference of the object

			// Boolean Fields
			private bool bIsExpandContract = false;             // bIsExpandContract: Returns if the object is performing an expand-contract sequence
			private bool bIsIdling = false;                     // bIsIdling: Returns if the object is performing an idling animation;
			private bool bIsIdlingRotation = false;             // bIsIdlingRotation: Returns if the object is performing an idling rotation animation

			// Constructor
			/// <summary>
			/// Enabling of animation effects
			/// </summary>
			/// <param name="_mTransform"> The transform of the object that is going to animate </param>
			public Animate(Transform _mTransform)
			{
				mTransform = _mTransform;
				vExpandContract_InitialScale = mTransform.localScale;
			}

			// Public Functions
			#region Expand-Contract Animation
			/// <summary>
			/// Performs the animation of the object expanding and contracting
			/// </summary>
			/// <param name="_fTimer"> The total time of the animation </param>
			/// <param name="_nFrequency"> The number of expansion and contraction it animates </param>
			/// <param name="_fSize"> The maximum size of the expansion, relative to the initial scale of the object when this method is called </param>
			/// <returns> Returns if the animation is executed </returns>
			public bool ExpandContract(float _fTimer, int _nFrequency, float _fSize)
			{
				return ExpandContract(_fTimer, _nFrequency, _fSize, true, false, 0.0f);
			}

			/// <summary>
			/// Performs the animation of the object expanding and contracting
			/// </summary>
			/// <param name="_fTimer"> The total time of the animation </param>
			/// <param name="_nFrequency"> The number of expansion and contraction it animates </param>
			/// <param name="_fSize"> The maximum size of the expansion, relative to the initial scale of the object when this method is called </param>
			/// <param name="_bIsOverridable"> Determines if this animation can be overriden by another function call </param>
			/// <param name="_bIsLoopForever"> Determines if this animation is going to loop forever </param>
			/// <param name="_fStartTime"> Use this function to start at a specific part of the animation. (0f = Start, 1f = End) </param>
			/// <returns> Returns if the animation is executed </returns>
			public bool ExpandContract(float _fTimer, int _nFrequency, float _fSize, bool _bIsOverridable, bool _bIsLoopForever, float _fStartTime)
			{
				// if: The animation is overridable
				if (bExpandContract_IsOverridable)
				{
					// if: It is not currently expanding and contracting, which means that this is a new animation and not an overriden one
					if (!bIsExpandContract)
					{
						vExpandContract_InitialScale = mTransform.localScale;
						// if: This animation cannot be passed to AnimateHandler.cs (cache is probably full)
						if (!AnimateHandler.ActivateExpandContract(this))
						{
							return false;
						}
					}

					// Initialisation of Expand-Contract Animation Fields
					fExpandContract_Timer = _fTimer;
					nExpandContract_Frequency = _nFrequency * 2;
					fExpandContract_Size = _fSize - 1f;
					fExpandContract_CurrentTimer = _fStartTime * fExpandContract_Timer;
					bExpandContract_IsOverridable = _bIsOverridable;
					bExpandContract_IsLoopForever = _bIsLoopForever;

					bIsExpandContract = true;
					return true;
				}
				return false;
			}

			/// <summary>
			/// Stops the current expand-contract animation
			/// </summary>
			/// <param name="_bHasExitTime"> 
			/// Determines if the stopping should have an exit time
			/// If true, the animation will play the last loop before ending it
			/// If false, the animation will stop immediately
			/// </param>
			/// <returns> If the function has been executed </returns>
			public bool StopExpandContract(bool _bHasExitTime)
			{
				// if: The object is expanding and contracting
				if (!bIsExpandContract)
					return false;

				// if: There is exit time
				if (_bHasExitTime)
				{
					// fTimePerCycle: The time taken to complete one cycle
					float fTimePerCycle = fExpandContract_Timer / (float)nExpandContract_Frequency * 2f;
					// fExtraTime: The length of time of the animation in the current cycle
					float fExtraTime = fExpandContract_CurrentTimer % fTimePerCycle;

					fExpandContract_CurrentTimer = fTimePerCycle * (float)(nExpandContract_Frequency / 2 - 1) + fExtraTime;
				}
				else
				{
					fExpandContract_CurrentTimer = fExpandContract_Timer;
				}
				bExpandContract_IsLoopForever = false;

				return true;
			}

			// __upEC(): The update call for expand-contract animation sequence.
			//           Returns true when sequence is not complete, false for otherwise
			public bool __upEC(float _fDeltaTime)
			{
				// Update current time
				fExpandContract_CurrentTimer += _fDeltaTime;
				if (bExpandContract_IsLoopForever)
					fExpandContract_CurrentTimer -= fExpandContract_Timer;

				if (fExpandContract_CurrentTimer < fExpandContract_Timer)
				{
					// fCompletion: Returns the value of completion (0.0f = Begin, 1.0f = End)
					float fCompletion = fExpandContract_CurrentTimer / fExpandContract_Timer;

					mTransform.localScale =
						vExpandContract_InitialScale * (1f + Mathf.PingPong(fCompletion * (float)nExpandContract_Frequency * fExpandContract_Size, fExpandContract_Size));
					return true;
				}
				else
				{
					ResetExpandContract();
					return false;
				}
			}

			// ResetExpandContract(): Resets related variables to before animation was called
			private void ResetExpandContract()
			{
				// Transform size back to default
				mTransform.localScale = vExpandContract_InitialScale;

				// Set all value back to zero
				fExpandContract_Timer = 0f;
				nExpandContract_Frequency = 0;
				fExpandContract_Size = 0f;
				fExpandContract_CurrentTimer = 0f;
				bExpandContract_IsOverridable = true;
				bExpandContract_IsLoopForever = false;

				bIsExpandContract = false;
			}
			#endregion



			#region Idle Animation
			/// <summary>
			/// Perform the animation for idling 
			/// </summary>
			/// <param name="_fSpeed"> The speed at which the object. It is advice to keep it small to simulate the effect </param>
			/// <param name="_fRadius"> The maximum distance away from the center at which the child can idle </param>
			/// <returns> Returns if the animation is executed </returns>
			public bool Idle(float _fSpeed, float _fRadius)
			{
				return Idle(_fSpeed, _fRadius, true, false, false);
			}

			/// <summary>
			/// Perform the animation for idling    
			/// </summary>
			/// <param name="_fSpeed"> The speed at which the object. It is advice to keep it small to simulate the effect </param>
			/// <param name="_fRadius"> The maximum distance away from the center at which the child can idle </param>
			/// <param name="_bIsOverridable"> Determines if this function call can be overiden by another function call </param>
			/// <param name="_bIsRestrictXAxis"> Set if the idling should not go along the x-axis </param>
			/// <param name="_bIsRestrictYAxis"> Set if the idling should not go along the y-axis </param>
			/// <returns> Returns if the animation is executed </returns>
			public bool Idle(float _fSpeed, float _fRadius, bool _bIsOverridable, bool _bIsRestrictXAxis, bool _bIsRestrictYAxis)
			{
				// if: The animation is overridable
				if (bIdle_IsOverridable)
				{
					// if: It is not currently idling, which means that this is a new animation and not an overriden one
					if (!bIsIdling)
					{
						vIdle_InitialPosition = mTransform.position;
						// if: This animation cannot be passed to AnimateHandler.cs (cache is probably full)
						if (!AnimateHandler.ActivateIdle(this))
						{
							return false;
						}
					}

					//Initialisation of Idling Fields
					fIdle_Speed = _fSpeed;
					fIdle_Radius = _fRadius;
					bIdle_IsOverridable = _bIsOverridable;
					vIdle_TargetPosition = mTransform.position; // Sets this to its current position so that that it can be set in update
					bIdle_RestrictXAxis = _bIsRestrictXAxis;
					bIdle_RestrictYAxis = _bIsRestrictYAxis;
					bIdle_IsComingToAHalt = false;

					bIsIdling = true;
					return true;
				}
				return false;
			}

			/// <summary>
			/// Stops the current idling animation
			/// </summary>
			/// <param name="_bHasExitTime"> 
			/// If exit time is enabled, the object will move back to its position before it stops.
			/// If exit time is disabled, the object will snap back to its original position immediately.
			/// </param>
			/// <returns> Returns if this function is executed </returns>
			public bool StopIdle(bool _bHasExitTime)
			{
				// if: The object is not idling
				if (!bIsIdling)
					return false;

				if (_bHasExitTime)
				{
					vIdle_TargetPosition = vIdle_InitialPosition;
					bIdle_IsComingToAHalt = true;
					return true;
				}
				else
				{
					ResetIdle();
					return false;
				}
			}

			// __upI(): The update call for idling animation sequence.
			//          Returns true when sequence is not complete, false for otherwise
			public bool __upI(float _deltaTime)
			{
				if (!bIsIdling)
					return false;

				float fDeltaSpeed = _deltaTime * fIdle_Speed;
				mTransform.position = Vector3.MoveTowards(mTransform.position, vIdle_TargetPosition, fDeltaSpeed);

				// if: The distance between the object and the target position is lesser than what it is moving - Use this to change new target position
				if (Vector3.Distance(mTransform.position, vIdle_TargetPosition) < fDeltaSpeed)
				{
					// if: Layman -> It is animating for 'Idle'
					if (!bIdle_IsComingToAHalt)
					{
						vIdle_TargetPosition = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f) * Vector3.up * fIdle_Radius + vIdle_InitialPosition;
					}
					// else: Layman -> It is animating for 'StopIdle'
					else
					{
						ResetIdle();
						return false;
					}
				}

				if (bIdle_RestrictXAxis)
					vIdle_TargetPosition.x = 0.0f;
				if (bIdle_RestrictYAxis)
					vIdle_TargetPosition.y = 0.0f;

				return true;
			}

			// ResetIdle(): Resets related variables to before animation was called
			private void ResetIdle()
			{
				mTransform.position = vIdle_InitialPosition;

				fIdle_Speed = 0f;
				fIdle_Radius = 0f;
				bIdle_IsOverridable = true;
				vIdle_TargetPosition = Vector3.zero;
				bIdle_RestrictXAxis = false;
				bIdle_RestrictYAxis = false;
				bIdle_IsComingToAHalt = false;

				bIsIdling = false;
			}
			#endregion



			#region Idle-Rotation Animation
			/// <summary>
			/// Perform the animation of idling-rotation
			/// </summary>
			/// <param name="_fMinRotation"> The minimum amount of angular rotation in a turn </param>
			/// <param name="_fMaxRotation"> The maximum amount of angular rotation in a turn </param>
			/// <param name="_fMinTime"> The minimum amount of time to execute a turn </param>
			/// <param name="_fMaxTime"> The maximum amount of time to execute a turn </param>
			/// <param name="_bIsSmoothTransition"> Determines if the transition between turns in smooth. NOTE: By enabling this the angles keyed are no longer accurate and will be used as torque </param>
			/// <param name="_bIsOverridable"> Determines if this function call can be overiden by another function call </param>
			/// <returns> Returns if the function is executed </returns>
			public bool IdleRotation(float _fMinRotation, float _fMaxRotation, float _fMinTime, float _fMaxTime, bool _bIsSmoothTransition, bool _bIsOverridable)
			{
				// if: The animation is overridable
				if (bIdleRotation_IsOverridable)
				{
					// if: It is not currently idling, which means that this is a new animation and not an overriden one
					if (!bIsIdlingRotation)
					{
						qIdleRotation_InitialQuaternion = mTransform.rotation;
						// if: Tries to push to cache and fails
						if (!AnimateHandler.ActivateIdleRotation(this))
						{
							return false;
						}
					}

					// Initialisation of variables
					fIdleRotation_MinRotation = _fMinRotation;
					fIdleRotation_RotationRange = Mathf.Abs(_fMaxRotation - _fMinRotation);
					fIdleRotation_MinTime = _fMinTime;
					fIdleRotation_TimeRange = Mathf.Abs(_fMaxTime - _fMinTime);
					bIdleRotation_IsOverridable = _bIsOverridable;
					fIdleRotation_CurrentTime = 0f;
					bIdleRotation_SmoothTransition = _bIsSmoothTransition;

					bIdle_IsComingToAHalt = false;
					bIsIdlingRotation = true;

					// Generates random values for rotation and time
					fIdleRotation_CurrentTotalRotation = fIdleRotation_MinRotation + (UnityEngine.Random.value * fIdleRotation_RotationRange);
					fIdleRotation_CurrentTotalTime = fIdleRotation_MinTime + (UnityEngine.Random.value * fIdleRotation_TimeRange);

					return true;
				}
				return false;
			}

			// __upI(): The update call for idling rotation animation sequence.
			//          Returns true when sequence is not complete, false for otherwise
			public bool __upIR(float _fDeltaTime)
			{
				// if: The object is not roational idling
				if (!bIsIdlingRotation)
					return false;

				fIdleRotation_CurrentTime += _fDeltaTime;

				// if: The current time have exceed the time taken for that turn
				if (fIdleRotation_CurrentTime >= fIdleRotation_CurrentTotalTime)
				{
					// if: THe current animation turn is running for 'StopIdleRotation' instead of 'IdleRotation'
					if (bIdleRotation_IsComingToAHalt)
					{
						// This where the stopping of the animation happens
						fIdleRotation_MinRotation = 0f;
						fIdleRotation_RotationRange = 0f;
						fIdleRotation_MinTime = 0f;
						fIdleRotation_TimeRange = 0f;
						bIdleRotation_IsOverridable = true;
						fIdleRotation_CurrentTime = 0f;
						bIdleRotation_SmoothTransition = false;
						bIdle_IsComingToAHalt = false;
						bIsIdlingRotation = false;

						Debug.Log(mTransform.rotation.eulerAngles);
						mTransform.rotation = qIdleRotation_InitialQuaternion;
						return false;
					}
					else
					{
						// Generates random values for rotation and time
						fIdleRotation_CurrentTotalRotation = fIdleRotation_MinRotation + (UnityEngine.Random.value * fIdleRotation_RotationRange);
						// Randomise between turning left or right
						fIdleRotation_CurrentTotalRotation *= Mathf.RoundToInt(UnityEngine.Random.value) == 0 ? -1f : 1f;

						fIdleRotation_CurrentTotalTime = fIdleRotation_MinTime + (UnityEngine.Random.value * fIdleRotation_TimeRange);
						fIdleRotation_CurrentTime -= fIdleRotation_CurrentTotalTime;
					}
				}
				// else: when the time has not exceed the time taken
				else
				{
					// fDeltaRotation: The fraction of rotation to be rotated in that current frame
					float fDeltaRotation = _fDeltaTime / fIdleRotation_CurrentTotalTime * fIdleRotation_CurrentTotalRotation;

					// if: Smooth transition is enabled, transit smoothly
					if (bIdleRotation_SmoothTransition)
					{
						fIdleRotation_CurrentTorque += _fDeltaTime / fIdleRotation_CurrentTotalTime * fIdleRotation_CurrentTotalRotation;
						fIdleRotation_CurrentTorque = Mathf.Min(Mathf.Abs(fIdleRotation_CurrentTorque), fDeltaRotation);
						mTransform.rotation = Quaternion.Euler(0f, 0f, fIdleRotation_CurrentTorque + mTransform.rotation.eulerAngles.z);
					}
					else
					{
						// Update the rotation of the transform
						mTransform.rotation = Quaternion.Euler(0f, 0f, fDeltaRotation + mTransform.rotation.eulerAngles.z);
					}

				}

				return true;

			}

			/// <summary>
			/// Stops the current rotationa idling animation 
			/// </summary>
			/// <param name="_bIsReturnToInitial"> Set if the object will rotate back to its original position before it stops </param>
			/// <returns> Returns if the function is executed </returns>
			public bool StopIdleRotation(bool _bIsReturnToInitial)
			{
				// if: The object is not rotational idling
				if (!bIsIdlingRotation)
					return false;

				if (_bIsReturnToInitial)
				{
					fIdleRotation_CurrentTotalRotation = Mathf.DeltaAngle(mTransform.rotation.eulerAngles.z, 0f);
					fIdleRotation_CurrentTime = 0f;
					bIdleRotation_IsComingToAHalt = true;
					bIdleRotation_SmoothTransition = false;
					//Debug.Log("mTransform.rotation.eulerAngles: " + mTransform.rotation.eulerAngles + ", fIdleRotation_CurrentTotalRotation: " + fIdleRotation_CurrentTotalRotation);
					return true;
				}
				else 
				{
					fIdleRotation_CurrentTime = fIdleRotation_CurrentTotalTime;
					return true;
				}
			}
			#endregion

			// Getter-Setter Functions
			public bool IsExpandContract { get { return bIsExpandContract; } }
			public bool IsIdling { get { return bIsIdling; } }
			public Transform AnimatedTransform { get { return mTransform; } }
		}
	}
}