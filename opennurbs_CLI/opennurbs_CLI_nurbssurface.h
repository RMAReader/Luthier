#pragma once

#include "opennurbs.h"
#include "opennurbs_CLI_point.h"

using namespace System;
using namespace System::Runtime::InteropServices;

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
		~NurbsSurface() { delete surface; surface = 0; }

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
			//int dim = surface->Dimension();

			//double* cpp_output = new double[surface->CVSize()];
			//ON_BOOL32 res = surface->Evaluate(u, v, nderiv, surface->CVSize(), cpp_output);
			//
			//array<double>^ output = gcnew array<double>(surface->CVSize());
			//for (int i = 0; i < surface->CVSize(); i++)
			//{
			//	output[i] = cpp_output[i];
			//}
			////delete[] cpp_output;
			//Marshal::FreeHGlobal(IntPtr(cpp_output));
			//return output;
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
		ON_BOOL32 SetKnot(int direction, int IX, double knot_value) { return surface->SetKnot(direction, IX, knot_value); }
		double GetKnot(int direction, int IX) { return surface->Knot(direction, IX); }

		ON_BOOL32 InsertKnot(int direction, double knot_value, int knot_multiplicity) { return surface->InsertKnot(direction, knot_value, knot_multiplicity); }
		ON_BOOL32 IncreaseDegree(int direction, int desired_degree) { return surface->IncreaseDegree(direction, desired_degree); }

		array<double>^ EvNormal( // returns false if unable to evaluate
			double u, double v   // evaluation parameters (s,t)
		)
		{
			ON_3dVector normal;
			ON_BOOL32 res = surface->EvNormal(u, v, normal);
			array<double>^ point = gcnew array<double>(3);
			if (res)
			{
				point[0] = normal[0];
				point[1] = normal[1];
				point[2] = normal[2];
			}
			return point;
		}
	};

}