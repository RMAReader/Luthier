#pragma once

#include "opennurbs.h"
#include "opennurbs_CLI_point.h"

using namespace System;

namespace opennurbs_CLI
{


	public ref class NurbsCurve
	{
	private:
		ON_NurbsCurve * curve;
		NurbsCurve(ON_NurbsCurve *source) { curve = source; }

	public:
		NurbsCurve(int dimension, ON_BOOL32 bIsRational, int order, int cv_count) { curve = ON_NurbsCurve::New(dimension, bIsRational, order, cv_count); }
		
		property int Dimension { int get() { return curve->Dimension(); }}
		property int Degree { int get() { return curve->Degree(); }}
		property int Order { int get() { return curve->Order(); }}
		property int CVCount { int get() { return curve->CVCount(); }}
		property int KnotCount { int get() { return curve->KnotCount(); }}
		property Interval^ Domain {Interval^ get(){return gcnew Interval(&(curve->Domain()));}}

		ON_BOOL32 Evaluate(double t, int nderiv, array<double>^ output)
		{
			pin_ptr<double> pinned_output = &output[0];
			return curve->Evaluate(t, nderiv, curve->CVSize(), pinned_output);
		}
		array<double>^ Evaluate(double t, int nderiv)
		{
			array<double>^ output = gcnew array<double>(curve->CVSize());
			pin_ptr<double> pinned_output = &output[0];
			ON_BOOL32 res = curve->Evaluate(t, nderiv, curve->CVSize(), pinned_output);
			return output;
		}
		ON_BOOL32 SetCV( int IX,int style,array<double>^ point)  
		{
			pin_ptr<double> pinned_point = &point[0];
			return curve->SetCV(IX, ON::PointStyle(style), pinned_point);
		}
		array<double>^ GetCV(int IX, int style)
		{
			array<double>^ point = gcnew array<double>(curve->CVSize());
			pin_ptr<double> pinned_point = &point[0];
			ON_BOOL32 res = curve->GetCV(IX, ON::PointStyle(style), pinned_point);
			return point;
		}
		bool SetKnot(int knot_index, double knot_value) { return curve->SetKnot(knot_index, knot_value); }
		double GetKnot(int knot_index) { return curve->Knot(knot_index); }
		
		
		bool InsertKnot(double knot_value, int knot_multiplicity) { return curve->InsertKnot(knot_value, knot_multiplicity); }
		bool IncreaseDegree(int desired_degree) { return curve->IncreaseDegree(desired_degree); }

	};

}