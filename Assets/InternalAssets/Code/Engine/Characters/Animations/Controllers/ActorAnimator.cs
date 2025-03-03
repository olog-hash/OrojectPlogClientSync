// dnSpy decompiler from Assembly-CSharp.dll class: ActorAnimator
using System;
using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
	public bool IsRunning
	{
		get
		{
			return this.keyState == KeyState.Runing || this.keyState == KeyState.RuningBack || this.keyState == KeyState.RunStrafeLeft || this.keyState == KeyState.RunStrafeRight;
		}
	}

	public bool IsWalking
	{
		get
		{
			return this.keyState == KeyState.Walking || this.keyState == KeyState.WalkBack || this.keyState == KeyState.WalkStrafeLeft || this.keyState == KeyState.WalkStrafeRight;
		}
	}

	public string IdleAnimationName
	{
		get
		{
			return this.idleAnimationName;
		}
		set
		{
			this.idleAnimationName = value;
		}
	}

	public string BendAnimationName
	{
		get
		{
			return this.bendAnimationName;
		}
		set
		{
			this.bendAnimationName = value;
		}
	}

	public AudioSource LegsAudio
	{
		get
		{
			if (this.legsAudio == null)
			{
				foreach (AudioSource audioSource in base.transform.GetComponentsInChildren<AudioSource>())
				{
					if (audioSource.gameObject.name == "Legs")
					{
						this.legsAudio = audioSource;
					}
				}
			}
			return this.legsAudio;
		}
	}

	public AudioSource SpeakerAudio
	{
		get
		{
			if (this.speakerAudio == null)
			{
				foreach (AudioSource audioSource in base.transform.GetComponentsInChildren<AudioSource>())
				{
					if (audioSource.gameObject.name == "Speaker")
					{
						this.speakerAudio = audioSource;
					}
				}
			}
			return this.speakerAudio;
		}
	}

	public void Refresh()
	{
		this.shopAnimator = false;
	}

	public void SetZombieAnimation(bool isZombie)
	{
		this.isZombie = isZombie;
	}

	public void PlayAnimation(string name, bool loop)
	{
		this.shopAnimator = true;
		if (loop)
		{
			base.GetComponent<Animation>()[name].wrapMode = WrapMode.Loop;
		}
		else
		{
			base.GetComponent<Animation>()[name].wrapMode = WrapMode.Once;
		}
		base.GetComponent<Animation>().Play(name, PlayMode.StopAll);
	}

	private void Start()
	{
	}


	private void SetupHeadAiming(string anim, bool mixingTransform)
	{
		if (mixingTransform)
		{
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck"), false);
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head"), false);
		}
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Blend;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 0f;
		base.GetComponent<Animation>()[anim].layer = 1;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
		base.GetComponent<Animation>()[anim].wrapMode = WrapMode.Once;
	}

	private void SetupAdditiveAiming(string anim, bool mixingTransform)
	{
		if (mixingTransform)
		{
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1"));
		}
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Blend;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 0f;
		base.GetComponent<Animation>()[anim].layer = 1;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
		base.GetComponent<Animation>()[anim].wrapMode = WrapMode.Once;
	}

	/*
	private void CheckSoldierState()
	{
		this.aim = this.playerRemote.Aim;
		this.fire = this.playerRemote.Fire;
		this.crouch = this.playerRemote.CrouchStatus;
		this.reloading = this.playerRemote.Reload;
		this.currentWeapon = (int)this.playerRemote.currentWeapon;
		this.inAir = this.playerRemote.InAir;
		this.currentWeaponName = ((this.playerRemote.currentWeapon != 0) ? "M203" : "M4");
		this.aimTarget = this.playerRemote.targetpos;
	}*/

	private float CrossFadeUp(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight + Time.deltaTime / fadeTime);
	}

	private float CrossFadeDown(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight - Time.deltaTime / fadeTime);
	}

	private void AnimationRemote()
	{
		//this.walk = this.playerRemote.Walk;
		//this.keyState = this.playerRemote.keyState;
	}

	private void AnimationAdaptive()
	{
		float num = (float)(DateTime.Now.Ticks - this.lastPositionTime) / 1E+07f;
		float num2 = (base.transform.localEulerAngles.y - this.lastRotation.y) / num;
		Vector3 direction = base.transform.position - this.lastPosition;
		if (direction.sqrMagnitude < this.IDLE_THRESHOLD && Mathf.Abs(num2) < this.ROTATION_IDLE_THRESHOLD)
		{
			if (DateTime.Now.Ticks - this.lastPositionTime > (long)this.IDLE_PERIOD)
			{
				this.keyState = KeyState.Still;
			}
			this.deltaZ = 0f; this.deltaR = (this.deltaX = (this.deltaZ ));
			return;
		}
		this.lastPositionTime = DateTime.Now.Ticks;
		if (Mathf.Abs(num2) < this.ROTATION_IDLE_THRESHOLD)
		{
			this.deltaR = 0f;
		}
		this.lastRotation = base.transform.localEulerAngles;
		this.deltaR = Mathf.LerpAngle(this.deltaR, num2, this.deltaRotationSmooth);
		if (direction.sqrMagnitude < this.IDLE_THRESHOLD)
		{
			if (this.deltaR > this.ROTATION_WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeRight;
				this.walk = false;
			}
			else if (this.deltaR > 0f)
			{
				this.keyState = KeyState.WalkStrafeRight;
				this.walk = true;
			}
			else if (this.deltaR < -this.ROTATION_WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeLeft;
				this.walk = false;
			}
			else if (this.deltaR < 0f)
			{
				this.keyState = KeyState.WalkStrafeLeft;
				this.walk = true;
			}
			this.deltaZ = 0f; this.deltaX = (this.deltaZ );
			return;
		}
		this.lastPosition = base.transform.position;
		Vector3 vector = base.transform.InverseTransformDirection(direction);
		float b = vector.x / num;
		float b2 = vector.z / num;
		this.deltaX = Mathf.Lerp(this.deltaX, b, this.deltaSmooth);
		this.deltaZ = Mathf.Lerp(this.deltaZ, b2, this.deltaSmooth);
		if (Mathf.Abs(this.deltaX) > Mathf.Abs(this.deltaZ) + this.STRAFE_THRESHOLD)
		{
			if (this.deltaX > this.WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeRight;
				this.walk = false;
			}
			else if (this.deltaX > 0f)
			{
				this.keyState = KeyState.WalkStrafeRight;
				this.walk = true;
			}
			else if (this.deltaX < -this.WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeLeft;
				this.walk = false;
			}
			else if (this.deltaX < 0f)
			{
				this.keyState = KeyState.WalkStrafeLeft;
				this.walk = true;
			}
			return;
		}
		if (this.deltaZ > this.WALK_THRESHOLD)
		{
			this.keyState = KeyState.Runing;
			this.walk = false;
		}
		else if (this.deltaZ > 0f)
		{
			this.keyState = KeyState.Walking;
			this.walk = true;
		}
		else if (this.deltaZ < -this.WALK_THRESHOLD)
		{
			this.keyState = KeyState.RuningBack;
			this.walk = false;
		}
		else if (this.deltaZ < 0f)
		{
			this.keyState = KeyState.WalkBack;
			this.walk = true;
		}
	}

	private void AnimationBlendKey()
	{
		switch (this.keyState)
		{
		case KeyState.Still:
			this.Idle();
			break;
		case KeyState.Walking:
			this.WalkFoward();
			break;
		case KeyState.Runing:
			this.Run();
			break;
		case KeyState.WalkBack:
			this.WalkBack();
			break;
		case KeyState.RuningBack:
			this.RunBack();
			break;
		case KeyState.WalkStrafeLeft:
			this.WalkStrafeLeft();
			break;
		case KeyState.WalkStrafeRight:
			this.WalkStrafeRight();
			break;
		case KeyState.RunStrafeLeft:
			this.RunStrafeLeft();
			break;
		case KeyState.RunStrafeRight:
			this.RunStrafeRight();
			break;
		}
	}

	/*
	private void FixedUpdate()
	{
		if (this.shopAnimator)
		{
			return;
		}
		if (!NetworkDev.Remote_Animation && (PlayerManager.Instance.LocalPlayer == null || this != PlayerManager.Instance.LocalPlayer.ActorAnimator))
		{
			this.AnimationAdaptive();
		}
	}

	private void Update()
	{
		if (this.shopAnimator || PlayerManager.Instance == null)
		{
			return;
		}
		if (NetworkDev.Remote_Animation || this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
		{
			this.AnimationRemote();
		}
		if (this.isRunning != this.IsRunning)
		{
			this.isRunning = this.IsRunning;
			WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>();
			if (componentsInChildren.Length > 0)
			{
				Transform parent = componentsInChildren[0].transform.parent;
				Animation component = parent.GetComponent<Animation>();
				if (component != null)
				{
					if (this.isRunning)
					{
						this.RunWeaponAnimation(component);
					}
					else
					{
						this.IdleWeaponAnimation(component);
					}
				}
			}
		}
		if (this.LegsAudio != null)
		{
			bool flag = this.isRunning && !this.inAir;
			if (PlayerManager.Instance.LocalPlayer != null && PlayerManager.Instance.LocalPlayer.ActorAnimator != this)
			{
				flag = (flag && (this.playerRemote.IsEnemy || !PlayerManager.Instance.LocalPlayer.ContainsEnhancer(EnhancerType.ClanReducedSoundFriend)));
			}
			if (flag)
			{
				SoundManager.Instance.Play(this.LegsAudio, "Run_Concrete", AudioPlayMode.PlayLoop);
			}
			else
			{
				SoundManager.Instance.Play(this.LegsAudio, "Run_Concrete", AudioPlayMode.Stop);
			}
		}
		if (this.isWalking != this.IsWalking)
		{
			this.isWalking = this.IsWalking;
		}
		this.CheckSoldierState();
		if (this.crouch)
		{
			this.crouchWeight = this.CrossFadeUp(this.crouchWeight, 0.4f);
		}
		else if (this.inAir && this.jumpLandCrouchAmount > 0f)
		{
			this.crouchWeight = this.CrossFadeUp(this.crouchWeight, 1f / this.jumpLandCrouchAmount);
		}
		else
		{
			this.crouchWeight = this.CrossFadeDown(this.crouchWeight, 0.45f);
		}
		float num = 1f - this.crouchWeight;
		if (this.fire)
		{
			this.aimWeight = this.CrossFadeUp(this.aimWeight, 0.2f);
			this.fireWeight = this.CrossFadeUp(this.fireWeight, 0.2f);
		}
		else if (this.aim)
		{
			this.aimWeight = this.CrossFadeUp(this.aimWeight, 0.3f);
			this.fireWeight = this.CrossFadeDown(this.fireWeight, 0.3f);
		}
		else
		{
			this.aimWeight = this.CrossFadeDown(this.aimWeight, 0.5f);
			this.fireWeight = this.CrossFadeDown(this.fireWeight, 0.5f);
		}
		float num2 = 1f - this.aimWeight;
		if (this.inAir)
		{
			this.groundedWeight = this.CrossFadeDown(this.groundedWeight, 0.1f);
		}
		else
		{
			this.groundedWeight = this.CrossFadeUp(this.groundedWeight, 0.2f);
		}
		if (this.soldierController != null)
		{
			this.BendAnimationAngle = this.soldierController.targetXRotation / 180f + 0.5f;
		}
		if (this.BendAnimationAngle < 0f)
		{
			this.BendAnimationAngle = 0f;
		}
		if (this.BendAnimationAngle > 0.96f)
		{
			this.BendAnimationAngle = 0.96f;
		}
		base.GetComponent<Animation>()[this.BendAnimationName].time = this.BendAnimationAngle;
		if (!this.taunting && !this.isZombie)
		{
			if (this.IsBendAnimation())
			{
				base.GetComponent<Animation>()[this.BendAnimationName].weight = 1f;
				base.GetComponent<Animation>().CrossFade(this.BendAnimationName, 0.2f);
			}
		}
		else
		{
			base.GetComponent<Animation>()[this.BendAnimationName].weight = 0f;
		}
		if (this.reloading)
		{
		}
		if (!this.inAir)
		{
			if (this.isZombie)
			{
				if (this.RunAnimationName != "Zombie_Run")
				{
					this.RunAnimationName = "Zombie_Run";
					this.StrafeRunLeftAnimationName = "Zombie_RunStrafeLeft";
					this.StrafeRunRightAnimationName = "Zombie_RunStrafeRight";
					this.RunBackwardsAnimationName = "Zombie_RunBackwards";
					this.IdleAnimationName = "Zombie_Idle";
					this.JumpAnimationName = "Zombie_Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
					base.GetComponent<Animation>()[this.BendAnimationName].weight = 0f;
				}
			}
			else if (this.crouch)
			{
				if (this.RunAnimationName != "CroachWalk")
				{
					this.RunAnimationName = "CroachWalk";
					this.StrafeRunLeftAnimationName = "CroachStrafeLeftWalk";
					this.StrafeRunRightAnimationName = "CroachStrafeRightWalk";
					this.RunBackwardsAnimationName = "CroachWalkBackwards";
					this.IdleAnimationName = "CroachIdle";
					this.JumpAnimationName = "Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
				}
			}
			else if (this.walk)
			{
				if (this.RunAnimationName != "Walk")
				{
					this.RunAnimationName = "Walk";
					this.StrafeRunLeftAnimationName = "StrafeLeftWalk";
					this.StrafeRunRightAnimationName = "StrafeRightWalk";
					this.RunBackwardsAnimationName = "WalkBackwards";
					this.IdleAnimationName = "Idle";
					this.JumpAnimationName = "Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
				}
			}
			else if (this.RunAnimationName != "Run2")
			{
				this.RunAnimationName = "Run2";
				this.StrafeRunLeftAnimationName = "StrafeRunLeft2";
				this.StrafeRunRightAnimationName = "StrafeRunRight2";
				this.RunBackwardsAnimationName = "RunBackwards";
				this.IdleAnimationName = "Idle";
				this.JumpAnimationName = "Jump";
				base.GetComponent<Animation>()[this.RunAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
			}
			if (this.jump)
			{
				this.JumpFinish(this.JumpAnimationName);
			}
			this.jump = false;
			this.AnimationBlendKey();
		}
		else
		{
			if (!this.jump)
			{
				if (this.playerRemote.keyState == KeyState.Runing || this.playerRemote.keyState == KeyState.RunStrafeLeft || this.playerRemote.keyState == KeyState.RunStrafeRight)
				{
					this.JumpStart(this.JumpAnimationName);
				}
				else
				{
					this.JumpStart(this.JumpAnimationName);
				}
			}
			this.jump = true;
			this.Jump();
		}
	}

	public void TestAnimation()
	{
		base.GetComponent<Animation>()[this.BendAnimationName].weight = 1f;
		UnityEngine.Debug.Log(string.Format("Animation Test", new object[0]));
		foreach (object obj in base.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.weight != 0f)
			{
				UnityEngine.Debug.Log(string.Format("Animation[\"{0}\"] w:{1} s:{2} t: {3}", new object[]
				{
					animationState.name,
					animationState.weight,
					animationState.speed,
					animationState.time
				}));
			}
		}
	}*/

	public void Fire(int Gun)
	{
	}

	public void Idle()
	{
		base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.IdleAnimationName);
	}

	public void Run()
	{
		base.GetComponent<Animation>()[this.RunAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.RunAnimationName);
	}

	public void RunBack()
	{
		base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.RunBackwardsAnimationName);
	}

	public void RunStrafeLeft()
	{
		base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.StrafeRunLeftAnimationName);
	}

	public void RunStrafeRight()
	{
		base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.StrafeRunRightAnimationName);
	}

	public void WalkFoward()
	{
		base.GetComponent<Animation>().CrossFade(this.RunAnimationName);
	}

	public void WalkBack()
	{
		base.GetComponent<Animation>().CrossFade(this.RunBackwardsAnimationName);
	}

	public void WalkStrafeLeft()
	{
		base.GetComponent<Animation>().CrossFade(this.StrafeRunLeftAnimationName);
	}

	public void WalkStrafeRight()
	{
		base.GetComponent<Animation>().CrossFade(this.StrafeRunRightAnimationName);
	}

	public void JumpStart(string jumpType)
	{
		base.GetComponent<Animation>()[jumpType].time = 0f;
		base.GetComponent<Animation>()[jumpType].weight = 1f;
		base.GetComponent<Animation>()[jumpType].speed = this.jumpAnimationSpeed;
		base.GetComponent<Animation>()[jumpType].time = 0f;
		base.GetComponent<Animation>().CrossFade(jumpType, 0.3f);
	}

	public void JumpFinish(string jumpType)
	{
	}

	public void Jump()
	{
	}

	/*
	public void Defeat(Vector3 shotImpulse)
	{
		
		base.GetComponent<Animation>().enabled = false;
		foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
		{
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
			rigidbody.sleepThreshold = this.sleepVelocity;
			Rigidbody rigidbody2 = rigidbody;
			float num = 1f;
			rigidbody.angularDrag = num;
			rigidbody2.drag = num;
			RigidBodyForce rigidBodyForce = rigidbody.GetComponent<RigidBodyForce>();
			if (rigidBodyForce == null)
			{
				rigidBodyForce = rigidbody.gameObject.AddComponent<RigidBodyForce>();
			}
			rigidBodyForce.Vector = new Vector3(0f, -100f, 0f);
			rigidBodyForce.ImpulseVector = shotImpulse;
			if (this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
			{
				rigidbody.GetComponent<Collider>().enabled = true;
			}
		}
	}

	public void Ressurrect()
	{
		this.jump = false;
		this.inAir = false;
		this.playerRemote.CrouchStatus = false;
		this.playerRemote.InAir = false;
		foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
		{
			if (rigidbody.gameObject.name == "Bip01")
			{
				rigidbody.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			rigidbody.isKinematic = true;
			rigidbody.useGravity = true;
			RigidBodyForce component = rigidbody.GetComponent<RigidBodyForce>();
			if (component != null)
			{
				component.Vector = new Vector3(0f, 0f, 0f);
			}
			ConstantForce component2 = rigidbody.GetComponent<ConstantForce>();
			if (component2 != null)
			{
				component2.force = new Vector3(0f, 0f, 0f);
			}
			if (this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
			{
				rigidbody.GetComponent<Collider>().enabled = false;
			}
		}
		base.GetComponent<Animation>().enabled = true;
	}
	*/

	private Vector3 lastPosition;

	private Vector3 lastRotation;

	private long lastPositionTime = DateTime.Now.Ticks;

	public int IDLE_PERIOD = 1000000;

	public float IDLE_THRESHOLD = 0.0001f;

	public float WALK_THRESHOLD = 10f;

	public float STRAFE_THRESHOLD;

	public float ROTATION_WALK_THRESHOLD = 100f;

	public float ROTATION_IDLE_THRESHOLD = 4f;

	public float deltaX;

	public float deltaZ;

	public float deltaR;

	public float deltaSmooth = 0.2f;

	public float deltaRotationSmooth = 0.2f;

	public string currentWeaponName;

	public float jumpLandCrouchAmount = 1.6f;

	public float sleepVelocity = 0.15f;

	public bool aim;

	public bool fire;

	public bool walk;

	public bool crouch;

	public bool reloading;

	public int currentWeapon;

	public bool inAir;

	public Vector3 aimTarget;

	public Transform aimPivot;

	private float aimAngleY;

	private float groundedWeight = 1f;

	private float crouchWeight;

	private float aimWeight;

	private float fireWeight;

	public bool isRunning;

	public float runAnimationSpeed = 1.3f;

	public float crouchAnimationSpeed = 1.1f;

	public float walkAnimationSpeed = 1.1f;

	public float jumpAnimationSpeed = 1f;

	public float idleAnimationSpeed = 1f;

	private bool taunting;

	public bool isWalking;

	private KeyState keyState;

	private bool jump;

	public string RunAnimationName = "Run2";

	public string StrafeRunLeftAnimationName = "StrafeRunLeft2";

	public string StrafeRunRightAnimationName = "StrafeRunRight2";

	public string RunBackwardsAnimationName = "RunBackwards";

	public string JumpAnimationName = "Jump";

	private string idleAnimationName = "Idle";

	private string bendAnimationName = "BatBendClip";

	public int RunAnimationType = 2;

	public float BendAnimationAngle = 0.5f;

	private bool setFlag;

	private bool isZombie;

	private AudioSource legsAudio;

	private AudioSource speakerAudio;

	private bool shopAnimator;
}
