#include "messwertstatistik.h"

MesswertStatistik::MesswertStatistik() {}

const double* MesswertStatistik::findeMaximum(const double* werte, int anzahl) {

    if (anzahl > 0 && werte != nullptr) {
        const double* max = &(werte[0]);
        for (int i = 1; i < anzahl; i++) {
            if (werte[i] > *max) {
                max = &(werte[i]);
            }
        }
        return max;
    }
    return nullptr;
}