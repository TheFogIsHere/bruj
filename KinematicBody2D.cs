using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
	private int speed = 200;
	private int gravity = 400;
	private float friction = .01f;
	private float acceleration = .5f;
	private int jumpheight = 700;
	private int dashspeed = 500;
	private bool isDashing = false;
	private float dashtimer = .2f;
	private float dashtimerreset = .2f;
	private bool isDashAvailable = true;
	Vector2 velocity = new Vector2();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(!isDashing){

			int direction = 0;
			if(Input.IsActionPressed("ui_left")){
				direction -= 1;
			}
			if(Input.IsActionPressed("ui_right")){
				direction += 1;
			}
			if(direction != 0){
				velocity.x = Mathf.Lerp(velocity.x, direction * speed, acceleration); //feel of real life friction/weighty feel
			}else{
				velocity.x = Mathf.Lerp(velocity.x, 0, friction);
			}
		}

		if(IsOnFloor()){
		if(Input.IsActionJustPressed("jump")){
				velocity.y -= jumpheight;
			}
			isDashAvailable = true;
		}

		if(isDashAvailable){
			if(Input.IsActionJustPressed("dash")){  //dashing
				if(Input.IsActionJustPressed("ui_left")){
					velocity.x = -dashspeed;
					isDashing = true;
				}
				if(Input.IsActionJustPressed("ui_right")){
					velocity.x = dashspeed;
					isDashing = true;
				}
				if(Input.IsActionJustPressed("ui_up")){
					velocity.y = -dashspeed;
					isDashing = true;
				}
				if(Input.IsActionJustPressed("ui_right")&& Input.IsActionJustPressed("ui_up")){
					velocity.x = dashspeed;
					velocity.y = -dashspeed;
					isDashing = true;
				}
				if(Input.IsActionJustPressed("ui_left")&& Input.IsActionJustPressed("ui_up")){
					velocity.x = -dashspeed;
					velocity.y = -dashspeed;
					isDashing = true;
				}

				dashtimer = dashtimerreset;
				isDashAvailable = false;
			}
		}
		if(isDashing){
			dashtimer -= delta;
			if(dashtimer <= 0){
				isDashing = false;
				velocity = new Vector2(0,0);
			}
		}else{
			velocity.y += gravity * delta;
		}

		
		MoveAndSlide(velocity, Vector2.Up);
	}
}
