#include <iostream>
#include <string>

using namespace std;

int main()
{
    int initialBoundary, interval, n;

    int lower;
    cout<<"Enter starting class boundaries:";
    cin>>initialBoundary;
    cout<<"Enter interval:";
    cin>>interval;
    cout<<"Enter no of observations: ";
    cin>>n;
    
    int classBoundary[n][2], f[n], x[n], fx[n];
    int totalF=0, totalFx=0;
    float arithmeticMean;
    
    lower=initialBoundary;
    for(int i=0; i<n; i++)
    {
        classBoundary[i][0]=lower;
        classBoundary[i][1]=classBoundary[i][0]+interval;
        lower=classBoundary[i][1];
        cout<<"Enter frequency of "<<classBoundary[i][0]<<"-"<<classBoundary[i][1]<<": ";
        cin>>f[i];
        x[i]=(classBoundary[i][0]+classBoundary[i][1])/2;
        fx[i]=f[i]*x[i];
        totalF+=f[i];
        totalFx+=fx[i];
    }
    arithmeticMean=totalFx/totalF;
    cout<<"Arithmetic Mean:"<<arithmeticMean;
    
    return 0;
}
