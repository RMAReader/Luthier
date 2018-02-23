


#include "opennurbs.h"

extern "C" {
	
	__declspec(dllexport) ON_NurbsCurve* ON_NurbsCurve_New()
	{
		return ON_NurbsCurve::New();
	}
	__declspec(dllexport) ON_NurbsCurve* ON_NurbsCurve_New2(int dimension,ON_BOOL32 bIsRational,int order,int cv_count)
	{
		return ON_NurbsCurve::New(dimension, bIsRational, order, cv_count);
	}
	__declspec(dllexport) ON_BOOL32 ON_NurbsCurve_Evaluate(ON_NurbsCurve* curve,double t,int derv_n,int stride, double* result,int evaluate_from = 0,int* evaluate_hint = 0 )
	{
		return curve->Evaluate(t, derv_n, stride, result, evaluate_from, evaluate_hint);
	}
	__declspec(dllexport) ON_BOOL32 ON_NurbsCurve_SetCV(ON_NurbsCurve* curve,int IX,int style,const double* value)
	{
		return curve->SetCV(IX, ON::PointStyle(style), value);
	}
	__declspec(dllexport) ON_BOOL32 ON_NurbsCurve_SetKnot(ON_NurbsCurve* curve, int IX, double value)
	{
		return curve->SetKnot(IX, value);
	}
	__declspec(dllexport) int ON_NurbsCurve_KnotCount(ON_NurbsCurve* curve)
	{
		return curve->KnotCount();
	}
	__declspec(dllexport) bool ON_NurbsCurve_GetCV(ON_NurbsCurve* curve, int IX, int style, double* value)
	{
		return curve->GetCV(IX, ON::PointStyle(style), value);
	}
	__declspec(dllexport) double ON_NurbsCurve_Knot(ON_NurbsCurve* curve, int IX)
	{
		return curve->Knot(IX);
	}
	__declspec(dllexport) void ON_NurbsCurve_Domain(ON_NurbsCurve* curve, double* domain)
	{
		auto interval = curve->Domain();
		domain[0] = interval.Min();
		domain[1] = interval.Max();
	}
	__declspec(dllexport) int ON_NurbsCurve_Degree(ON_NurbsCurve* curve)
	{
		return curve->Degree();
	}
	__declspec(dllexport) bool ON_NurbsCurve_InsertKnot(ON_NurbsCurve* curve, double knot_value, int knot_multiplicity)
	{
		return curve->InsertKnot(knot_value, knot_multiplicity);
	}
	__declspec(dllexport) bool ON_NurbsCurve_IncreaseDegree(ON_NurbsCurve* curve, int desired_degree)
	{
		return curve->IncreaseDegree(desired_degree);
	}
	__declspec(dllexport) bool ON_NurbsCurve_GetLength(ON_NurbsCurve* curve, double* length, double fractional_tolerance, const double* sub_domain)
	{
		return curve->GetLength(length, fractional_tolerance, &ON_Interval(sub_domain[0], sub_domain[1]));
	}
	__declspec(dllexport) int ON_NurbsCurve_CVCount(ON_NurbsCurve* curve)
	{
		return curve->CVCount();
	}
	__declspec(dllexport) bool ON_NurbsCurve_GetClosestPoint(ON_NurbsCurve* curve, const double* p, double* t, double maximum_distance, const double* sub_domain)
	{
		return curve->GetClosestPoint(ON_3dPoint(p),t,maximum_distance, &ON_Interval(sub_domain[0], sub_domain[1]));
	}

	
}