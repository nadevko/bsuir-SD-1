DESTDIR ?= dist
LABS ?= $(foreach lab,$(filter-out dist/,$(sort $(dir $(wildcard */)))),$(lab:/=))

NETS := $(foreach lab,$(LABS),$(wildcard $(lab)/Program.cs))
OUTS := $(foreach net,$(NETS),$(DESTDIR)/$(dir $(net)))
PDFS := $(foreach lab,$(LABS),$(DESTDIR)/report/lr-$(lab).pdf)

### Phony ######################################################################

.PHONY: all dist-% clean clean-all clean-dist clean-build

all: $(DESTDIR)

clean: clean-dist clean-build

clean-dist:
	@rm --verbose --recursive --force $(DESTDIR)

clean-build:
	@rm --verbose --recursive --force $(foreach lab,$(LABS),$(lab)/build $(lab)/_minted-* $(lab)/svg-inkscape)

### Build general ##############################################################

define mk-dir =
$(DESTDIR)/$(1)/:
	@mkdir --verbose --parent $(DESTDIR)/$(1)/
endef

$(DESTDIR): $(OUTS) $(PDFS)

### Build code #################################################################

define mk-lab =
$(DESTDIR)/$(basename $(1)): | $(DESTDIR)/$(dir $(1)) $(1)
	$(CXX) $(CXXFLAGS) -o $(DESTDIR)/$(basename $(1)) $(1)
endef

$(foreach lab,$(LABS),$(eval $(call mk-dir,$(lab))))

$(foreach cpp,$(CPPS),$(eval $(call mk-lab,$(cpp))))

### Build reports ##############################################################

define mk-report =
$(1)/build/report.pdf: $(1)/report.tex
	-cd $(1) && latexmk -synctex=1 -interaction=nonstopmode -file-line-error -xelatex -shell-escape -outdir=build -f report.tex

$(DESTDIR)/report/lr-$(1).pdf: | $(DESTDIR)/report/ $(1)/build/report.pdf
	@cp --verbose $(1)/build/report.pdf $(DESTDIR)/report/lr-$(1).pdf
endef

$(eval $(call mk-dir,report))

$(foreach lab,$(LABS),$(eval $(call mk-report,$(lab))))
