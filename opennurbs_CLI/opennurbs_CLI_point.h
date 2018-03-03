#pragma once
#include "opennurbs.h"

using namespace System;

namespace opennurbs_CLI
{

	public ref class Interval
	{
	private:
		ON_Interval * interval;


	public:
		Interval() { interval = new ON_Interval(); }
		Interval(ON_Interval *source) { interval = new ON_Interval(source->m_t[0], source->m_t[1]); }
		Interval(double t0, double t1) { interval = new ON_Interval(t0, t1); };
		~Interval() { delete interval; interval = 0; }

		property double Min { double get() { return interval->Min(); } }
		property double Max { double get() { return interval->Max(); } }
		void Set(double t0, double t1) { interval->Set(t0, t1); }
	};
	
	
	public ref class Point2d
	{
	private:
		ON_2dPoint * point;


	public:
		property double X { double get() { return point->x; } }
		property double Y { double get() { return point->y; } }

	};


}