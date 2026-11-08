# Subdirs-Projekt: baut zuerst die Factory-Bibliothek,
# dann die Plugins und zuletzt die App.
TEMPLATE = subdirs

CONFIG += ordered

SUBDIRS += \
    Formen_Factory \
    Formen_Plugin \
    Formen_Plugin_Eigen \
    Formen
