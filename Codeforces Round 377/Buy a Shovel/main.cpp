﻿#include <iostream>
#include <algorithm>
using namespace std;
int main()
{
	ios::sync_with_stdio(false);
	int k, r;
	cin >> k >> r;
	for (int i = 1;; i++)
		if (k*i % 10 == 0 || k*i % 10 == r)
		{
			cout << i << endl;
			return 0;
		}
}
