Shader "Myshaders/TransparentUnlit" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Texture", 2D) = "white" { }
}
SubShader { 
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 26345
Program "vp" {
SubProgram "opengl " {
GpuProgramIndex 0
}
SubProgram "d3d9 " {
GpuProgramIndex 1
}
SubProgram "d3d11 " {
GpuProgramIndex 2
}
SubProgram "d3d11_9x " {
GpuProgramIndex 3
}
SubProgram "opengl " {
GpuProgramIndex 4
}
SubProgram "d3d9 " {
GpuProgramIndex 5
}
SubProgram "d3d11 " {
GpuProgramIndex 6
}
SubProgram "d3d11_9x " {
GpuProgramIndex 7
}
SubProgram "opengl " {
GpuProgramIndex 8
}
SubProgram "d3d9 " {
GpuProgramIndex 9
}
SubProgram "d3d11 " {
GpuProgramIndex 10
}
SubProgram "d3d11_9x " {
GpuProgramIndex 11
}
}
Program "fp" {
SubProgram "opengl " {
GpuProgramIndex 12
}
SubProgram "d3d9 " {
GpuProgramIndex 13
}
SubProgram "d3d11 " {
GpuProgramIndex 14
}
SubProgram "d3d11_9x " {
GpuProgramIndex 15
}
SubProgram "opengl " {
GpuProgramIndex 16
}
SubProgram "d3d9 " {
GpuProgramIndex 17
}
SubProgram "d3d11 " {
GpuProgramIndex 18
}
SubProgram "d3d11_9x " {
GpuProgramIndex 19
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 110926
Program "vp" {
SubProgram "opengl " {
GpuProgramIndex 20
}
SubProgram "d3d9 " {
GpuProgramIndex 21
}
SubProgram "d3d11 " {
GpuProgramIndex 22
}
SubProgram "d3d11_9x " {
GpuProgramIndex 23
}
SubProgram "opengl " {
GpuProgramIndex 24
}
SubProgram "d3d9 " {
GpuProgramIndex 25
}
SubProgram "d3d11 " {
GpuProgramIndex 26
}
SubProgram "d3d11_9x " {
GpuProgramIndex 27
}
SubProgram "opengl " {
GpuProgramIndex 28
}
SubProgram "d3d9 " {
GpuProgramIndex 29
}
SubProgram "d3d11 " {
GpuProgramIndex 30
}
SubProgram "d3d11_9x " {
GpuProgramIndex 31
}
SubProgram "opengl " {
GpuProgramIndex 32
}
SubProgram "d3d9 " {
GpuProgramIndex 33
}
SubProgram "d3d11 " {
GpuProgramIndex 34
}
SubProgram "d3d11_9x " {
GpuProgramIndex 35
}
SubProgram "opengl " {
GpuProgramIndex 36
}
SubProgram "d3d9 " {
GpuProgramIndex 37
}
SubProgram "d3d11 " {
GpuProgramIndex 38
}
SubProgram "d3d11_9x " {
GpuProgramIndex 39
}
}
Program "fp" {
SubProgram "opengl " {
GpuProgramIndex 40
}
SubProgram "d3d9 " {
GpuProgramIndex 41
}
SubProgram "d3d11 " {
GpuProgramIndex 42
}
SubProgram "d3d11_9x " {
GpuProgramIndex 43
}
SubProgram "opengl " {
GpuProgramIndex 44
}
SubProgram "d3d9 " {
GpuProgramIndex 45
}
SubProgram "d3d11 " {
GpuProgramIndex 46
}
SubProgram "d3d11_9x " {
GpuProgramIndex 47
}
SubProgram "opengl " {
GpuProgramIndex 48
}
SubProgram "d3d9 " {
GpuProgramIndex 49
}
SubProgram "d3d11 " {
GpuProgramIndex 50
}
SubProgram "d3d11_9x " {
GpuProgramIndex 51
}
SubProgram "opengl " {
GpuProgramIndex 52
}
SubProgram "d3d9 " {
GpuProgramIndex 53
}
SubProgram "d3d11 " {
GpuProgramIndex 54
}
SubProgram "d3d11_9x " {
GpuProgramIndex 55
}
SubProgram "opengl " {
GpuProgramIndex 56
}
SubProgram "d3d9 " {
GpuProgramIndex 57
}
SubProgram "d3d11 " {
GpuProgramIndex 58
}
SubProgram "d3d11_9x " {
GpuProgramIndex 59
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 179789
Program "vp" {
SubProgram "opengl " {
GpuProgramIndex 60
}
SubProgram "d3d9 " {
GpuProgramIndex 61
}
SubProgram "d3d11 " {
GpuProgramIndex 62
}
SubProgram "d3d11_9x " {
GpuProgramIndex 63
}
SubProgram "opengl " {
GpuProgramIndex 64
}
SubProgram "d3d9 " {
GpuProgramIndex 65
}
SubProgram "d3d11 " {
GpuProgramIndex 66
}
SubProgram "d3d11_9x " {
GpuProgramIndex 67
}
SubProgram "opengl " {
GpuProgramIndex 68
}
SubProgram "d3d9 " {
GpuProgramIndex 69
}
SubProgram "d3d11 " {
GpuProgramIndex 70
}
SubProgram "d3d11_9x " {
GpuProgramIndex 71
}
SubProgram "opengl " {
GpuProgramIndex 72
}
SubProgram "d3d9 " {
GpuProgramIndex 73
}
SubProgram "d3d11 " {
GpuProgramIndex 74
}
SubProgram "d3d11_9x " {
GpuProgramIndex 75
}
}
Program "fp" {
SubProgram "opengl " {
GpuProgramIndex 76
}
SubProgram "d3d9 " {
GpuProgramIndex 77
}
SubProgram "d3d11 " {
GpuProgramIndex 78
}
SubProgram "d3d11_9x " {
GpuProgramIndex 79
}
SubProgram "opengl " {
GpuProgramIndex 80
}
SubProgram "d3d9 " {
GpuProgramIndex 81
}
SubProgram "d3d11 " {
GpuProgramIndex 82
}
SubProgram "d3d11_9x " {
GpuProgramIndex 83
}
SubProgram "opengl " {
GpuProgramIndex 84
}
SubProgram "d3d9 " {
GpuProgramIndex 85
}
SubProgram "d3d11 " {
GpuProgramIndex 86
}
SubProgram "d3d11_9x " {
GpuProgramIndex 87
}
SubProgram "opengl " {
GpuProgramIndex 88
}
SubProgram "d3d9 " {
GpuProgramIndex 89
}
SubProgram "d3d11 " {
GpuProgramIndex 90
}
SubProgram "d3d11_9x " {
GpuProgramIndex 91
}
}
 }
}
Fallback "Transparent/Diffuse"
}