// testAtn2.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file

// https://gist.github.com/volkansalma/2972237

#include <iostream>
#include <chrono>
#define M_PI 3.14159265358979323846264338327950288

float atan2_approximation1(float y, float x)
{
	//http://pubs.opengroup.org/onlinepubs/009695399/functions/atan2.html
	//Volkan SALMA

	const float ONEQTR_PI = M_PI / 4.0;
	const float THRQTR_PI = 3.0 * M_PI / 4.0;
	float r, angle;
	float abs_y = fabs(y) + 1e-10f;      // kludge to prevent 0/0 condition
	if (x < 0.0f)
	{
		r = (x + abs_y) / (abs_y - x);
		angle = THRQTR_PI;
	}
	else
	{
		r = (x - abs_y) / (x + abs_y);
		angle = ONEQTR_PI;
	}
	angle += (0.1963f * r * r - 0.9817f) * r;
	if (y < 0.0f)
		return(-angle);     // negate if in quad III or IV
	else
		return(angle);


}

#define PI_FLOAT     3.14159265f
#define PIBY2_FLOAT  1.5707963f
// |error| < 0.005
float atan2_approximation2(float y, float x)
{
	if (x == 0.0f)
	{
		if (y > 0.0f) return PIBY2_FLOAT;
		if (y == 0.0f) return 0.0f;
		return -PIBY2_FLOAT;
	}
	float atan;
	float z = y / x;
	if (fabs(z) < 1.0f)
	{
		atan = z / (1.0f + 0.28f*z*z);
		if (x < 0.0f)
		{
			if (y < 0.0f) return atan - PI_FLOAT;
			return atan + PI_FLOAT;
		}
	}
	else
	{
		atan = PIBY2_FLOAT - z / (z*z + 0.28f);
		if (y < 0.0f) return atan - PI_FLOAT;
	}
	return atan;
}

// -x > -y >= 0, so divide by 0 not possible
static signed char iat2(signed char y, signed char x) {
	// printf("x=%4d y=%4d\n", x, y); fflush(stdout);
	return ((y * 32 + (x / 2)) / x) * 2;  // 3.39 mxdiff
	// return ((y*64+(x/2))/x);    // 3.65 mxdiff
	// return (y*64)/x;            // 3.88 mxdiff
}

signed char iatan2sc(signed char y, signed char x) {
	// determine octant
	if (y >= 0) { // oct 0,1,2,3
		if (x >= 0) { // oct 0,1
			if (x > y) {
				return iat2(-y, -x) / 2 + 0 * 32;
			}
			else {
				if (y == 0) return 0; // (x=0,y=0)
				return -iat2(-x, -y) / 2 + 2 * 32;
			}
		}
		else { // oct 2,3
	   // if (-x <= y) {
			if (x >= -y) {
				return iat2(x, -y) / 2 + 2 * 32;
			}
			else {
				return -iat2(-y, x) / 2 + 4 * 32;
			}
		}
	}
	else { // oct 4,5,6,7
		if (x < 0) { // oct 4,5
		  // if (-x > -y) {
			if (x < y) {
				return iat2(y, x) / 2 + -4 * 32;
			}
			else {
				return -iat2(x, y) / 2 + -2 * 32;
			}
		}
		else { // oct 6,7
	   // if (x <= -y) {
			if (-x >= y) {
				return iat2(-x, y) / 2 + -2 * 32;
			}
			else {
				return -iat2(y, -x) / 2 + -0 * 32;
			}
		}
	}
}

#include <math.h>

static void test_iatan2sc(signed char y, signed char x) {
	static int mn = INT_MAX;
	static int mx = INT_MIN;
	static double mxdiff = 0;

	signed char i = iatan2sc(y, x);
	static const double Pi = 3.1415926535897932384626433832795;
	double a = atan2(y ? y : -0.0, x) * 256 / (2 * Pi);

	if (i < mn) {
		mn = i;
		//printf("x=%4d,y=%4d  --> %4d   %f, mn %d mx %d mxdiff %f\n",
		//	x, y, i, a, mn, mx, mxdiff);
	}
	if (i > mx) {
		mx = i;
		//printf("x=%4d,y=%4d  --> %4d   %f, mn %d mx %d mxdiff %f\n",
		//	x, y, i, a, mn, mx, mxdiff);
	}

	double diff = fabs(i - a);
	if (diff > 128) diff = fabs(diff - 256);

	if (diff > mxdiff) {
		mxdiff = diff;
		//printf("x=%4d,y=%4d  --> %4d   %f, mn %d mx %d mxdiff %f\n",
		//	x, y, i, a, mn, mx, mxdiff);
	}
}

int main()
{
    std::cout << "Testing atan2 atan2_aprox1 and atan2_aprox2 in C++\n";
	float x = 1;
	float y = 0;

	auto t1 = std::chrono::high_resolution_clock::now();

	for (y = 0; y < 2 * M_PI; y += 0.1)
	{
		for (x = 0; x < 2 * M_PI; x += 0.1)
		{
			float angle = atan2(y, x);
//			printf("atan2 for %f,%f: %f \n", y, x, angle);
		}
	}
	auto t2 = std::chrono::high_resolution_clock::now();
	auto duration1 = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();
	t1 = std::chrono::high_resolution_clock::now();

	for (y = 0; y < 2 * M_PI; y += 0.1)
	{
		for (x = 0; x < 2 * M_PI; x += 0.1)
		{
			float angle = atan2_approximation1(y, x);
			//printf("approx1 for %f,%f: %f \n", y, x, angle);
		}
	}
	
	t2 = std::chrono::high_resolution_clock::now();
	auto duration2 = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();
	t1 = std::chrono::high_resolution_clock::now();

	for (y = 0; y < 2 * M_PI; y += 0.1)
	{
		for (x = 0; x < 2 * M_PI; x += 0.1)
		{
			float angle = atan2_approximation2(y, x);
//			printf("approx2 for %f,%f: %f \n \n", y, x, atan2_approximation2(y, x));
		}
	}
	t2 = std::chrono::high_resolution_clock::now();
	auto duration3 = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();

	std::cout << "Elapsed time - atan2: " << duration1 << " - aprox1: " << duration2 << " - aprox2: " << duration3 << "\n";
	
	int x1, y1;
	int n = 127;
	t1 = std::chrono::high_resolution_clock::now();

	for (y1 = -n - 1; y1 <= n; y1++) {
		for (x1 = -n - 1; x1 <= n; x1++) {
			test_iatan2sc(y1, x1);
		}
	}
	t2 = std::chrono::high_resolution_clock::now();
	duration1 = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();

	puts("Done");
	std::cout << "Elapsed time - test_iatan2sc: " << duration1 << "\n";


	return 0;
}


