#ifndef REGISTER_H
#define REGISTER_H

#include <QtCore/qglobal.h>
#include "factory.h"

#if defined(PLUGIN_LIBRARY)
#  define PLUGIN Q_DECL_EXPORT
#else
#  define PLUGIN Q_DECL_IMPORT
#endif

// Jede Plugin-DLL exportiert diese eine C-Funktion.
extern "C" PLUGIN bool registerForm(Factory *factory);

#endif // REGISTER_H
