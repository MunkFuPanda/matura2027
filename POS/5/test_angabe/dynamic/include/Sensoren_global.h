#ifndef SENSOREN_GLOBAL_H
#define SENSOREN_GLOBAL_H

#include <QtCore/qglobal.h>

#if defined(SENSOREN_LIBRARY)
#define SENSOREN_EXPORT Q_DECL_EXPORT
#else
#define SENSOREN_EXPORT Q_DECL_IMPORT
#endif

#endif // SENSOREN_GLOBAL_H
