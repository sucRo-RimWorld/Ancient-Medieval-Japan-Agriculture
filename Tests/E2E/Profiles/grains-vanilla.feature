Feature: Grains real-provider migration smoke - vanilla

  Scenario: Grains vanilla loads the requested real providers
    Then the real vanilla Grains providers are active

  Scenario: Grains vanilla primary grain loop resolves
    Then the Grains primary grain loop resolves

  Scenario: Grains vanilla optional cold tolerance resolves
    Then Grains cold tolerance extensions are absent

  Scenario: Grains vanilla wheat flour and minimum food resolve
    Then the Grains wheat flour and minimum food chains resolve

  Scenario: Grains vanilla seven grains retain environmental niches
    Then seven Grains retain environmental harvest niches

  @quickstart:AmjStageAQuickstart @timeout:270
  Scenario: Grains vanilla real harvest and flour food Bills complete
    Then Grains harvest and flour food Bills complete through real jobs

  @quickstart:AmjStageAQuickstart @timeout:270
  Scenario: Grains vanilla common food single and mixed Bills complete
    Then Grains common food single and mixed Bills complete through real jobs

  @quickstart:AmjStageAQuickstart @timeout:270
  Scenario: Grains vanilla V1 alternate processing benches complete
    Then Grains V1 alternate processing benches complete native Bills

  @quickstart:AmjStageAQuickstart @timeout:270
  Scenario: Grains vanilla cooking Bills reject wrong ingredients
    Then Grains cooking Bills reject wrong ingredient families through native work givers

  @quickstart:AmjStageAQuickstart @timeout:270
  Scenario: Grains vanilla food ingestion applies correct thoughts
    Then Grains common and specialty foods apply actual ingestion thoughts
