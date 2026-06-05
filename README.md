# PUXdesign_DirectoryWatcher

Vytvoril jsem jednoduchy program pro detekci soubotu ve reozitaru. Obsluha programu je pomoci jednoducheho REST API s 1 endpointem: /api/watcher/scan?path=C:/path . endpoint vraci o neco vic informaci nez bylo pozadovano v zadani informace ktere nas zajimaji jsou 

seznam nových souborů a podadresářů - added and addedDir
seznam změněných souborů - modified
seznam odstraněných souborů a podadresářů - removed and removedDir

Pro detekci zmeny se program diva na last modified datum v metadatech souboru. Pro tento testovaci poripad mi tento zpusob pripada dostacuujici, ovsem pro realen pouziti by melo byt implemntovano trackovani pomoci hashu nebo nejaky jiny zpusob - rozhodne v teto oblasti nejsem expert.

Nastaveni kde se ukaldaji snapshoty jednotlivych cest jsou take ulozeny je take dostacujici pouze pro testovaci ukol a melo by byt zmeneno na nejaky nastavitleny a globalni zpusob.

Snapshoty jsou ulozeny v JSON. Testovani bylo provedeno puze pomoci jednoduchych souboru, pri pouziti mene obvyklich souboru se program muze chovat nepreedvidatelne. 

K reseni bylo pouzito AI vicemene pro konzultaci, vyttrvoreni struktury a set-up programu (Program.cs je vygenerovany) logiku samotneho souboru jsem napsal az an drobnosti bez AI.