#pragma once

#include "opennurbs.h"
#include "opennurbs_CLI_point.h"

using namespace System;

namespace opennurbs_CLI
{


	public ref class NurbsSurface
	{
	private:
		ON_NurbsSurface * surface;

	public:
		NurbsSurface() { surface = ON_NurbsSurface::New(); }
		NurbsSurface(int dimension,
			ON_BOOL32 bIsRational,
			int order0,
			int order1,
			int cv_count0,
			int cv_count1
		) { surface = ON_NurbsSurface::New(dimension, bIsRational, order0,order1, cv_count0, cv_count1); }
	
		property int Dimension { int get() { return surface->Dimension(); }}
		int Degree(int direction) { return surface->Degree(direction); }
		int Order(int direction) { return surface->Order(direction); }
		property int CVCount { int get() { return surface->CVCount(); }}
		int KnotCount(int direction) { return surface->KnotCount(direction); }
		Interval^ Domain(int direction) {return gcnew Interval(&(surface->Domain(direction))); }
	
		array<double>^ Evaluate(double u, double v, int nderiv)
		{
			array<double>^ output = gcnew array<double>(surface->CVSize());
			pin_ptr<double> pinned_output = &output[0];
			ON_BOOL32 res = surface->Evaluate(u,v, nderiv, surface->CVSize(), pinned_output);
			return output;
		}
	
		ON_BOOL32 SetCV(int IU, int IV, int style, array<double>^ point)
		{
			pin_ptr<double> pinned_point = &point[0];
			return surface->SetCV(IU, IV, ON::PointStyle(style), pinned_point);
		}
		array<double>^ GetCV(int IU, int IV, int style)
		{
			array<double>^ point = gcnew array<double>(surface->CVSize());
			pin_ptr<double> pinned_point = &point[0];
			ON_BOOL32 res = surface->GetCV(IU, IV, ON::PointStyle(style), pinned_point);
			return point;
		}
		bool SetKnot(int direction, int IX, double knot_value) { return surface->SetKnot(direction, IX, knot_value); }
		double GetKnot(int direction, int IX) { return surface->Knot(direction, IX); }

		bool InsertKnot(int direction, double knot_value, int knot_multiplicity) { return surface->InsertKnot(direction, knot_value, knot_multiplicity); }
		bool IncreaseDegree(int direction, int desired_degree) { return surface->IncreaseDegree(direction, desired_degree); }


	};

}