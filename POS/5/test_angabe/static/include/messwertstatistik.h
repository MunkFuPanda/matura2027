#ifndef MESSWERTSTATISTIK_H
#define MESSWERTSTATISTIK_H

class MesswertStatistik
{
public:
    MesswertStatistik();
    const double* findeMaximum(const double* werte, int anzahl);
};

#endif // MESSWERTSTATISTIK_H
