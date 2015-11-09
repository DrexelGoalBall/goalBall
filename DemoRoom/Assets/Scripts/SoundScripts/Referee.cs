using UnityEngine;
using System.Collections;

public class Referee : MonoBehaviour {

	public AudioSource quiet;
	public AudioSource begin;
	public AudioSource penalty;
	public AudioSource half;
	public AudioSource score;


	public void Quiet(){
		quiet.Play();
	}

	public void Begin(){
		begin.Play();
	}
	
	public void Penalty(){
		penalty.Play();
	}

	public void Half(){
		half.Play();
	}

	public void Score(){
		score.Play();
	}
}
