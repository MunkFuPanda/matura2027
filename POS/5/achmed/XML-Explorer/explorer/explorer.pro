QT += widgets

CONFIG += c++17

TARGET = XmlExplorer
TEMPLATE = app

# Import aus der dynamischen TinyXml2-Bibliothek (nur Windows)
win32:DEFINES += TINYXML2_IMPORT

INCLUDEPATH += $$PWD/../tinyxml2lib
LIBS += -L$$OUT_PWD -ltinyxml2

DEPENDPATH += $$PWD/../tinyxml2lib

SOURCES += \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    mainwindow.h

FORMS += \
    mainwindow.ui

# Linux: Bibliothek liegt neben der App (Windows: DLL neben .exe ohnehin Auffindbar)
unix: QMAKE_RPATHDIR += $$OUT_PWD
