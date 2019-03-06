﻿using UnityEngine;

// utilizing the GRAVITY DISPLACEMENT METHOD

public static class Helpers {
	public static void SetPositionX(Transform t, float x) {
		t.position = new Vector3 (x, t.position.y, t.position.z);
	}
	public static void SetPositionY(Transform t, float y) {
		t.position = new Vector3 (t.position.x, y, t.position.z);
	}
	public static void SetPositionZ(Transform t, float z) {
		t.position = new Vector3 (t.position.x, t.position.y, z);
	}
}
